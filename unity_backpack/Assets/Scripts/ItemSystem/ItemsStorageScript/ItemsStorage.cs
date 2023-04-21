using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsStorage : MonoBehaviour
{
    [SerializeField] protected string _storeName;
    [SerializeField] protected int _maxSize;
    [SerializeField] protected uint _unlock_size;
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

    }
    
    public void AddItem(AssetItem newItem) {
        _items.Add(newItem);
    }

    public void RemoveItem(ItemCell cellFrom) {

    }

    public AssetItem SwapItem(ItemCell cellFrom, AssetItem importItem) {
        return importItem;
    }


}
