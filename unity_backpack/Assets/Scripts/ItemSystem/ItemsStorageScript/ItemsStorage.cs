using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsStorage : MonoBehaviour
{
    [SerializeField] protected string _storeName;
    [SerializeField] protected int _maxSize;
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

    public abstract void RenderBack();
    public abstract void RenderItems();
    
    public abstract void AddItem(AssetItem newItem) {
        _items.Add(newItem);
    }
    public abstract void RemoveItem();
    public abstract AssetItem SwapItem(AssetItem importItem);


}
