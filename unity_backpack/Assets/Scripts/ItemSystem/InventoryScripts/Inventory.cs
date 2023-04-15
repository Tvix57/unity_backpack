using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


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
    [SerializeField] private Transform _containerBackground;
    [SerializeField] private Transform _draggingParent;
    [SerializeField] private uint _max_size = 30;
    [SerializeField] private uint _unlock_size = 15;
    [SerializeField] private uint _items_in_row = 5;

    public void Start() {
        SetSize();
        _items = db.LoadItems("Inventory");
        RenderItems(_items);
    }

    void OnApplicationQuit() {
        db.SaveItems("Inventory", _items);
    }

    public void OnEnable() {
        RenderItems(Items);
        RenderBack();
    }

    private void SetSize() {
        GameObject parentObject = GameObject.Find("Inventory");
        GridLayoutGroup[] gridLayouts = parentObject.GetComponentsInChildren<GridLayoutGroup>();
        float cell_width = ((parentObject.GetComponent<RectTransform>().rect.width - 10) / _items_in_row)-(gridLayouts[0].spacing.x);
        foreach (GridLayoutGroup gridLayout in gridLayouts) {
            gridLayout.cellSize = new Vector2(cell_width, cell_width);
        }
    }

    public void RenderItems(List<AssetItem> items) {
        foreach (Transform child in _container) {
            Destroy(child.gameObject);
        }
        items.ForEach(item => {
            var cell = Instantiate(_inventoryShower, _container);
            cell.Init(_draggingParent);
            cell.Render(item);
            cell.InInventory += () => ReplaceItem(item, cell);
            cell.Drop += () => Destroy(cell.gameObject);
            cell.Drop += () => Items.Remove(item);
            cell.Eqip += () => TryEquip(item, cell);
        });
    }

    public void RenderBack() {
        foreach (Transform child in _containerBackground) {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < _max_size; ++i) {
            var cell = Instantiate(_inventoryCell, _containerBackground);
            if (i < _unlock_size) {
                cell.Render(true);
            } else {
                cell.Render(false);
            }
        }
    }

    public void AddItem(AssetItem item) {
        if (Items.Count < _unlock_size) {
            Items.Add(item);
            RenderItems(Items);
        }
    }

    public void RemoveItem() {
        if (Items.Count != 0) {
            Items.RemoveAt(UnityEngine.Random.Range(0, Items.Count));
            RenderItems(Items);
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
        for (int i = 0; i < Items.Count; ++i) {
            if (Items[i].Type == IItem.ItemType.Consumables) {
                ConsumablesAssetItem cons = Items[i] as ConsumablesAssetItem;
                if (cons.ConsumableType == _ammo_type) {
                    if (cons.Count > 1) {
                        --cons.Count;
                    } else {
                        Items.RemoveAt(i);
                    }
                    result = true;
                    RenderItems(Items);
                    break;
                }
            }
        }
        return result;
    }

    public void ReplaceItem(AssetItem item, InventoryShower shower) {
        Vector3 mousePosition = Input.mousePosition;
        if (Items.Contains(item)) {
            Items.Remove(item);
        }
        shower.transform.parent = _container.transform;

        for(int i = 0; i < _container.transform.childCount; i++) {
            var child = _container.GetChild(i);
            if (child.GetComponent<RectTransform>().rect.Contains(
                child.transform.InverseTransformPoint(mousePosition))) {
                if (item.Stackable && item.ID == Items[i].ID) {
                    StackItem(Items[i], item);
                    Destroy(shower.gameObject);
                } else {
                    Items.Insert(++i, item);
                    shower.transform.SetSiblingIndex(i);
                }
                RenderItems(Items);
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

    public void TryEquip(AssetItem item, InventoryShower shower) {
        AssetItem prev_item = myDelegate(item, shower);
        if (item != prev_item) {
            Items.Remove(item);
            if (prev_item != null) {
                AddItem(prev_item);
            }
        }
        RenderItems(Items);
    }
}

