using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsStorage : MonoBehaviour
{
    [SerializeField] protected string _storeName;
    [SerializeField] protected int _maxSize;
    [SerializeField] protected int _unlock_size;
    [SerializeField] protected List<AssetItem> _items;
    [SerializeField] protected ItemCell _cellPrefab;
    [SerializeField] protected ItemShower _showerPrefab;
    [SerializeField] protected DatabaseManager _db;
    [SerializeField] protected Transform _cellContainer;
    [SerializeField] protected Transform _draggingParent;

    public void Start() {
        _items = _db.LoadItems(_storeName);
    }

    public void OnApplicationQuit() {
        _db.SaveItems(_storeName, _items);
    }

    public void RenderBack() {
        for (int i = 0; i < _maxSize; ++i) {
            var cell = Instantiate(_cellPrefab, _cellContainer);
            if (i < _unlock_size) {
                cell.Render(true);
            } else {
                cell.Render(false);
            }
        }
    }

    public void RenderItems() {
        if (index <_cellContainer.transform.childCount && index < Items.Count) {
            var cell = _cellContainer.GetChild(index).GetComponent<ItemCell>();
            LinkItem(Items[index] , cell);
        }
    }
    
    private void LinkItem(AssetItem item_asset, ItemCell cell) {
        if (item_asset != null) {
            var item = Instantiate(_showerPrefab);
            item.Init(_draggingParent);
            item.Render(item_asset);
            cell.SetItem(item);
            item.InInventory += () => ReplaceItem(item_asset, item);
            item.Drop += () => Destroy(item.gameObject);
            item.Drop += () => Items.Remove(item_asset);
            item.Eqip += () => TryEquip(item_asset, item);
        }
    }

    public void RenderItem() {

    }

    public void AddItem(AssetItem newItem) {
        _items.Add(newItem);
    }

    public void RemoveItem(ItemCell cellFrom) {

    }

    public AssetItem SwapItem(ItemCell cellFrom, ItemShower importItem) {
        return importItem;
    }

    public void ReplaceItem(AssetItem item, ItemShower shower) {
        Vector3 mousePosition = Input.mousePosition;
        if (Items.Contains(item)) {
            Items.Remove(item);
        }
        for (int i = 0; i < _cellContainer.transform.childCount; ++i) {
            var child = _cellContainer.GetChild(i);
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


    private void StackItem(AssetItem ToItem, AssetItem FromItem) {
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

}
