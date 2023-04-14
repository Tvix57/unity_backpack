using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Inventory Invetory =>_inventory;

    public uint Armor { 
        get { 
            uint arm = 0;
            if (_head != null) {
                arm += _head.Armor;
            }
            if (_body != null) {
                arm += _body.Armor;
            }
            return arm; 
        }
    }
    public uint Damage { 
        get { 
            if (_weapon != null) {
                return _weapon.Damage;
            } else {
                return 0;
            }  
        }
    }

    [SerializeField] private Inventory _inventory;
    // [SerializeField] private InventoryShower _inventoryShower;
    private ArmorAssetItem _head = null;
    private ArmorAssetItem _body = null;
    private GunAssetItem _weapon = null;
    
    public void Start() {
        _inventory.myDelegate = SetItem;
    }

    public void Shoot() {
        if (_weapon != null && _inventory.GetAmmo(_weapon.AmmoType)) {
            Debug.Log("Shoot");
        } else {
            Debug.Log("No ammo");
        }
    }
    public AssetItem SetItem(AssetItem item) {
        AssetItem prev_item = null;
        switch(item.Slot) {
            case IItem.ItemSlot.Head: {
                prev_item = _head;
                _head = item as ArmorAssetItem; break; 
            }
            case IItem.ItemSlot.Body: { 
                prev_item = _body;
                _body = item as ArmorAssetItem; break; 
                }
            case IItem.ItemSlot.Hands: { 
                prev_item = _weapon;
                _weapon = item as GunAssetItem; break; 
            }
        }
        return prev_item;
    }
}
