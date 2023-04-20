using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public Transform ItemContainer { get { return _container; } }

    [SerializeField] private DatabaseManager db;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private ItemShower _inventoryShower;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _containerBackground;
    [SerializeField] private Transform _draggingParent;

    private ArmorAssetItem _head = null;
    private ArmorAssetItem _body = null;
    private GunAssetItem _weapon = null;
    
    public void Start() {
        _inventory.myDelegate = SetItem;
        LoadFromDB();
    }

    void OnApplicationQuit() {
        LoadToDB();
    }

    public void LoadFromDB() {
        List<AssetItem> items = db.LoadItems("Equipment");
        items.ForEach(item => {
            ItemShower shower;
            switch (item.Slot) {
                case IItem.ItemSlot.Head: { 
                    _head = item as ArmorAssetItem;
                    shower = Instantiate(_inventoryShower, transform.Find("Content").Find("HeadSlot"));;
                    break; 
                }
                case IItem.ItemSlot.Body: { 
                    _body = item as ArmorAssetItem;
                    shower = Instantiate(_inventoryShower, transform.Find("Content").Find("BodySlot"));
                    break; 
                }
                case IItem.ItemSlot.Hands: {
                    _weapon = item as GunAssetItem; 
                    shower = Instantiate(_inventoryShower, transform.Find("Content").Find("GunSlot"));
                    break; 
                }
                default: { shower = null; break; }
            }
            shower?.Init(_draggingParent);
            shower?.Render(item);
        });
    }

    public void LoadToDB() {
        List<AssetItem> items = new List<AssetItem>();
        if (_head != null) items.Add(_head);
        if (_body != null) items.Add(_body);
        if (_weapon != null) items.Add(_weapon);
        db.SaveItems("Equipment", items);
    }
    public void Shoot() {
        if (_weapon != null && _inventory.GetAmmo(_weapon.AmmoType)) {
            Debug.Log("Shoot" + Damage.ToString());
        } else {
            Debug.Log("No ammo");
        }
    }

    public AssetItem SetItem(AssetItem item, ItemShower inputShower) {
        AssetItem prev_item = null;
        Vector3 mousePosition = Input.mousePosition;
        for (int i = 0; i < _container.transform.childCount; ++i) {
            var child = _container.GetChild(i);
            if (child.GetComponent<RectTransform>().rect.Contains(
                child.transform.InverseTransformPoint(mousePosition))) {
                if (item.Slot == child.gameObject.GetComponent<SlotProperty>().SlotType) {
                    if (child.childCount != 0) {
                        foreach (Transform childShower in child.transform) {
                            Destroy(childShower.gameObject);
                        }
                    }
                    inputShower.transform.parent = child.transform;
                    inputShower.transform.position = child.transform.position;  

                    switch (item.Slot) {
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
                } else {
                    prev_item = item;
                    Destroy(inputShower.gameObject);
                }
                break;
            }
        }
        return prev_item;
    }
}
