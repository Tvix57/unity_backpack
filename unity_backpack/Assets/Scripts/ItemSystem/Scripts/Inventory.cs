using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<AssetItem> Items;
    [SerializeField] private InventoryShower _inventoryShower;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _draggingParent;
    [SerializeField] private uint _max_size = 30;
    [SerializeField] private uint _unlock_size = 15;
    [SerializeField] private uint _widht_in_items = 5;

    public void OnEnable() {
        Render(Items);        
    }

    public void Render(List<AssetItem> items) {
        foreach (Transform child in _container) {
            Destroy(child.gameObject);
        }
        items.ForEach(item => {
            var cell = Instantiate(_inventoryShower, _container);
            cell.Init(_draggingParent);
            cell.Render(item);
        });
    } 

    public void AddItem(AssetItem item) {
        if (Items.Count < _unlock_size) {
            Items.Add(item);
            Render(Items);
        }
    }

    public void RemoveItem() {
        if (Items.Count != 0) {
            Items.RemoveAt(UnityEngine.Random.Range(0, Items.Count - 1));
            Render(Items);
        }
    }

    public void AddRandomItem() {

        var new_item = new AssetItem();
        AddItem(new_item);
    }

    public void AddAmmo() {
        List<AssetItem> ammos;
        foreach (AssetItem item in ammos) {
            if (Items.Count < _unlock_size) {
                new_item.Count = new_item.Max_stack;
                AddItem(new_item);
            } else {
                break;
            }
        }
        Render(Items);
    }

    public void Shoot() {
        //find ammo and -1
        Render(Items);
    }
}
 