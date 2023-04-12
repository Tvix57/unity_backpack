using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<AssetItem> Items;
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
}
 
