using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;
using static UnityEditor.Progress;


public delegate AssetItem EquipDelegate(AssetItem item, InventoryShower shower);

public class Inventory : MonoBehaviour
{
    public EquipDelegate myDelegate;

    public Transform ItemContainer { get { return _container; } }

    public List<AssetItem> Items => _items;


    [SerializeField] private DatabaseManager db;
    [SerializeField] private List<AssetItem> _items;
    [SerializeField] private InventoryShower _inventoryShower;
    [SerializeField] private InventoryCell _inventoryCell;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _draggingParent;
    [SerializeField] private uint _max_size = 30;
    [SerializeField] private uint _unlock_size = 15;
    [SerializeField] private uint _items_in_row = 5;

    public void Start() {
        SetSize();
        _items = db.LoadItems("Inventory");
        RenderAll();
    }

    void OnApplicationQuit() {
        db.SaveItems("Inventory", _items);
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

    public void RenderAll() {
        RenderBack();
        RenderItems();
    }

    public void RenderItems() {
        for (int i = 0; i < _container.transform.childCount; ++i) {
            var cell = _container.GetChild(i).GetComponent<InventoryCell>();
            if (i < _items.Count) {
                if (cell.Status) {
                    var item = Instantiate(_inventoryShower);
                    item.Init(_draggingParent);
                    item.Render(_items[i]);
                    cell.SetItem(item);
                    item.InInventory += () => ReplaceItem(_items[i], item);
                    item.Drop += () => Destroy(item.gameObject);
                    item.Drop += () => _items.Remove(_items[i]);
                    item.Eqip += () => TryEquip(_items[i], item);
                }
            } else {
                cell.RemoveItem();
            }
        }
    }

    public void RenderItems(int index_from) {
        for (int i = index_from; i < _container.transform.childCount; ++i) {
            var cell = _container.GetChild(i).GetComponent<InventoryCell>();
            if (i < _items.Count) {
                if (cell.Status) {
                    var item = Instantiate(_inventoryShower);
                    item.Init(_draggingParent);
                    item.Render(_items[i]);
                    cell.SetItem(item);
                    item.InInventory += () => ReplaceItem(_items[i], item);
                    item.Drop += () => Destroy(item.gameObject);
                    item.Drop += () => _items.Remove(_items[i]);
                    item.Eqip += () => TryEquip(_items[i], item);
                }
            } else {
                cell.RemoveItem();
            }
        }
    }

    public void ReRenderItem(int index) {

        var cell = _container.GetChild(index).GetComponent<InventoryCell>();
        var item = Instantiate(_inventoryShower);
        item.Init(_draggingParent);
        item.Render(_items[index]);
        cell.SetItem(item);

        item.InInventory += () => ReplaceItem(_items[index], item);
        item.Drop += () => Destroy(item.gameObject);
        item.Drop += () => _items.Remove(_items[index]);
        item.Eqip += () => TryEquip(_items[index], item);
    }

    public void RenderBack() {
        for (int i = 0; i < _max_size; ++i) {
            var cell = Instantiate(_inventoryCell, _container);
            if (i < _unlock_size) {
                cell.Render(true);
            } else {
                cell.Render(false);
            }
        }
    }

    public void AddItem(AssetItem item) {
        if (_items.Count < _unlock_size) {
            _items.Add(item);
            RenderItems();
        }
    }

    public void RemoveRandomItem() {
        if (_items.Count != 0) {
            int index = UnityEngine.Random.Range(0, _items.Count);
            _items.RemoveAt(index);
            RemoveAllItems(index);
            RenderItems(index);
        }
    }

    public void AddRandomItem() {
        AssetItem[] allItems = Resources.LoadAll<AssetItem>("Prefabs/ItemsBase/Equipment");
        if (allItems.Length != 0) {
            int randomIndex = UnityEngine.Random.Range(0, allItems.Length);
            AssetItem randomItem = Instantiate(allItems[randomIndex]);
            AddItem(randomItem);
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
        for (int i = 0; i < _items.Count; ++i) {
            if (_items[i].Type == IItem.ItemType.Consumables) {
                ConsumablesAssetItem cons = _items[i] as ConsumablesAssetItem;
                if (cons.ConsumableType == _ammo_type) {
                    if (cons.Count > 1) {
                        --cons.Count;
                    } else {
                        _items.RemoveAt(i);
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

    public void ReplaceItem(AssetItem item, InventoryShower shower) {
        Vector3 mousePosition = Input.mousePosition;
        if (_items.Contains(item)) {
            _items.Remove(item);
        }

        for (int i = 0; i < _container.transform.childCount; ++i) {
            var child = _container.GetChild(i);
            if (child.GetComponent<RectTransform>().rect.Contains(
                child.transform.InverseTransformPoint(mousePosition))) {
  
                if (item.Stackable && item.ID == _items[i].ID) {
                    StackItem(_items[i], item);
                    Destroy(shower.gameObject);
                } else {
                    _items.Insert(++i, item);
                    child.GetComponent<InventoryCell>().SetItem(shower);
                    shower.transform.SetSiblingIndex(i);
                }
                RemoveAllItems(i);
                RenderItems(i);
                break;
            }
        }
    }

    private void RemoveAllItems() {
        for (int i = 0; i < _container.transform.childCount; ++i) {
            _container.GetChild(i).GetComponent<InventoryCell>().RemoveItem();
        }
    }

    private void RemoveAllItems(int index_from) {
        for (int i = index_from; i < _container.transform.childCount; ++i) {
            _container.GetChild(i).GetComponent<InventoryCell>().RemoveItem();
        }
    }

    public void StackItem(AssetItem ToItem, AssetItem FromItem) {
        uint new_count = ToItem.Count + FromItem.Count;
        if (new_count > ToItem.Max_stack) {
            ToItem.Count = ToItem.Max_stack;
            FromItem.Count = new_count - ToItem.Max_stack;
            _items.Add(FromItem);
        } else {
            ToItem.Count = new_count;
            _items.Remove(FromItem);
        }
    }

    public void TryEquip(AssetItem item, InventoryShower shower) {
        AssetItem prev_item = myDelegate(item, shower);
        if (item != prev_item) {
            _items.Remove(item);
            if (prev_item != null) {
                AddItem(prev_item);
            }
        }
        RemoveAllItems();
        RenderItems();
    }
}

