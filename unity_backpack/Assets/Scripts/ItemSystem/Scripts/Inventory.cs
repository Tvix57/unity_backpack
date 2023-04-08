using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<AssetItem> Items;
    [SerializeField] private InventoryShower _inventoryShower;
    [SerializeField] private Transform _container;
    public void OnEnable() {
        Render(Items);        
    }

    public void Render(List<AssetItem> items) {
        foreach (Transform child in _container) {
            Destroy(child.gameObject);
        }
        items.ForEach(item => {
            var cell = Instantiate(_inventoryShower, _container);
            cell.Render(item);
        });
    } 
}
 