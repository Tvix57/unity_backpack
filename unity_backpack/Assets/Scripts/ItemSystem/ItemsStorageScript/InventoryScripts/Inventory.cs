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
    

    public List<AssetItem> Items => _items;

    [SerializeField] private uint _items_in_row;


    public void OnEnable() {
       RenderItems();
    }

    protected override void SetSize()  {
        GameObject parentObject = GameObject.Find("Inventory");
        GridLayoutGroup[] gridLayouts = parentObject.GetComponentsInChildren<GridLayoutGroup>();
        float cell_width = ((parentObject.GetComponent<RectTransform>().rect.width - 10) / _items_in_row)-(gridLayouts[0].spacing.x);
        foreach (GridLayoutGroup gridLayout in gridLayouts) {
            gridLayout.cellSize = new Vector2(cell_width, cell_width);
        }
    }

    public void RemoveRandomItem() {
        List<ItemCell> celsWithItems = new List<ItemCell>();
        foreach (Transform child in transform) {
            ItemCell item = child.GetComponent<ItemCell>();
            if (item != null && item.Item != null) {
                celsWithItems.Add(item);
            }
        }
        if (celsWithItems.Count != 0) {
            celsWithItems[UnityEngine.Random.Range(0, celsWithItems.Count)].RemoveItem();
        }
    }

    public void AddRandomItem() {
        AssetItem[] allItems = Resources.LoadAll<AssetItem>("Prefabs/ItemsBase/Equipment");
        if (allItems.Length != 0) {
            AddItem(Instantiate(allItems[UnityEngine.Random.Range(0, allItems.Length)]));
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
                 //  RemoveAllItems(i);
                //    RenderItems(i);
                    result = true;
                    break;
                }
            }
        }
        return result;
    }


/*    public void TryEquip(AssetItem item, ItemShower shower)
    {
        AssetItem prev_item = myDelegate(item, shower);
        if (item != prev_item)
        {
            Items.Remove(item);
            if (prev_item != null)
            {
                AddItem(prev_item);
            }
        }
        //   RemoveAllItems();
        RenderItems();
    }*/



}

