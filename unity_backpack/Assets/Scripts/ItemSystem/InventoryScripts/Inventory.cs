using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using UnityEditorInternal.Profiling.Memory.Experimental;
using static UnityEditor.Progress;


public delegate AssetItem EquipDelegate(AssetItem item, ItemShower shower);

public class Inventory : ItemsStorage
{
    public EquipDelegate myDelegate;

    public Transform ItemContainer { get { return _container; } }

    public List<AssetItem> Items => _items;

    [SerializeField] private uint _unlock_size;
    [SerializeField] private uint _items_in_row;

    public void Start() {
        SetSize();
        RenderAll();
    }

    public void OnApplicationQuit() {
    }

    public void OnEnable() {
       RenderItems();
    }

    private void SetSize() {
        GameObject parentObject = GameObject.Find("Inventory");
        GridLayoutGroup[] gridLayouts = parentObject.GetComponentsInChildren<GridLayoutGroup>();
        float cell_width = ((parentObject.GetComponent<RectTransform>().rect.width - 10) / _items_in_row)-(gridLayouts[0].spacing.x);
        foreach (GridLayoutGroup gridLayout in gridLayouts) {
            gridLayout.cellSize = new Vector2(cell_width, cell_width);
        }
    }

    private void RenderAll() {
        RenderBack();
        RenderItems();
    }

    private void RenderItems(int index_from = 0) {
        for (int i = index_from; i < _container.transform.childCount; ++i) {
            var cell = _container.GetChild(i).GetComponent<ItemCell>();
            if (i < Items.Count) {
                if (cell.Status) {
                    LinkItem(Items[i], cell);
                }
            } else {
                cell.RemoveItem();
            }
        }
    }

    private void RenderItem(int index) {
        if (index <_container.transform.childCount && index < Items.Count) {
            var cell = _container.GetChild(index).GetComponent<ItemCell>();
            LinkItem(Items[index] , cell);
        }
    }

    private void LinkItem(AssetItem item_asset, ItemCell cell) {
        var item = Instantiate(_showerPrefab);
        item.Init(_draggingParent);
        item.Render(item_asset);
        cell.SetItem(item);

        item.InInventory += () => ReplaceItem(item_asset, item);
        item.Drop += () => Destroy(item.gameObject);
        item.Drop += () => Items.Remove(item_asset);
        item.Eqip += () => TryEquip(item_asset, item);
    }

    private void RenderBack() {
        for (int i = 0; i < _maxSize; ++i) {
            var cell = Instantiate(_cellPrefab, _container);
            if (i < _unlock_size) {
                cell.Render(true);
            } else {
                cell.Render(false);
            }
        }
    }

    private void RemoveAllItems(int index_from = 0) {
        for (int i = index_from; i < _container.transform.childCount; ++i) {
            _container.GetChild(i).GetComponent<ItemCell>().RemoveItem();
        }
    }

    public void AddItem(AssetItem item) {
        if (Items.Count < _unlock_size) {
            Items.Add(item);
            RenderItem(Items.Count - 1);
        }
    }

    public void RemoveRandomItem() {
        if (Items.Count != 0) {
            int index = UnityEngine.Random.Range(0, Items.Count);
            Items.RemoveAt(index);
            RemoveAllItems(index);
            RenderItems(index);
        }
    }

    public void AddRandomItem() {
        AssetItem[] allItems = Resources.LoadAll<AssetItem>("Prefabs/ItemsBase/Equipment");
        if (allItems.Length != 0) {
            int randomIndex = UnityEngine.Random.Range(0, allItems.Length);
            AddItem(Instantiate(allItems[randomIndex]));
        }
    }

    public void AddAmmo() {
        AssetItem[] allItems = Resources.LoadAll<AssetItem>("Prefabs/ItemsBase/Consumables");
        for (int i = 0; i < allItems.Length; ++i) {
            if (Items.Count < _unlock_size) {
                AssetItem ammo = Instantiate(allItems[i]);
                ammo.Count = ammo.Max_stack;
                AddItem(ammo);
            } else {
                break;
            }
        }
    }

    public bool GetAmmo(ConsumablesAssetItem.Type _ammo_type) {
        bool result = false;
        for (int i = 0; i < Items.Count; ++i) {
            if (Items[i].Type == IItem.ItemType.Consumables) {
                ConsumablesAssetItem cons = Items[i] as ConsumablesAssetItem;
                if (cons.ConsumableType == _ammo_type) {
                    if (cons.Count > 1) {
                        --cons.Count;
                    } else {
                        Items.RemoveAt(i);
                    }
                    RemoveAllItems(i);
                    RenderItems(i);
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    public void ReplaceItem(AssetItem item, ItemShower shower) {
        Vector3 mousePosition = Input.mousePosition;
        if (Items.Contains(item)) {
            Items.Remove(item);
        }
        for (int i = 0; i < _container.transform.childCount; ++i) {
            var child = _container.GetChild(i);
            if (child.GetComponent<RectTransform>().rect.Contains(
                child.transform.InverseTransformPoint(mousePosition))) {
                    
                if (i < Items.Count) {
                    if (item.Stackable && item.ID == Items[i].ID) {
                        StackItem(Items[i], item);
                    } else {
                        Items.Insert(i, item);
                    }
                } else {
                    Items.Add(item);
                }
                Destroy(shower.gameObject);
                RemoveAllItems();
                RenderItems();
                break;
            }
        }
    }

    public void StackItem(AssetItem ToItem, AssetItem FromItem) {
        uint new_count = ToItem.Count + FromItem.Count;
        if (new_count > ToItem.Max_stack) {
            ToItem.Count = ToItem.Max_stack;
            FromItem.Count = new_count - ToItem.Max_stack;
            Items.Add(FromItem);
        } else {
            ToItem.Count = new_count;
            Items.Remove(FromItem);
        }
    }

    public void TryEquip(AssetItem item, ItemShower shower) {
        AssetItem prev_item = myDelegate(item, shower);
        if (item != prev_item) {
            Items.Remove(item);
            if (prev_item != null) {
                AddItem(prev_item);
            }
        }
        RemoveAllItems();
        RenderItems();
    }
}

