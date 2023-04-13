using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventoryShower : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public event Action Injecting;

    [SerializeField] private Text _nameField;
    [SerializeField] private Image _icon;
    [SerializeField] private Text _count;

    private Transform _draggingParent;
    private Transform _originalParent;

    public void Init(Transform draggingParent) {
        _draggingParent = draggingParent;
        _originalParent = transform.parent;
    }

    public void Render(IItem item) {
        _nameField.text = item.Name;
        _icon.sprite = item.Icon;
        _icon.preserveAspect = true;
        if (_count != null) {
            if (item.Stackable && item.Count > 1) {
                _count.text = item.Count.ToString();
            } else {
                _count.text = "";
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        transform.parent = _draggingParent;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }
    
    public void OnEndDrag(PointerEventData eventData) {
        // eventData.position = Input.mousePosition;
        // List<RaycastResult> results = new List<RaycastResult>();
        // EventSystem.current.RaycastAll(eventData, results);
        // gameObject parent_obj = _originalParent.parent;

        // if (results.Count > 0) {
        //     foreach(RaycastResult result in results) {
        //         if (result.gameObject.name == "Inventory") {
        //             if () {
        //                 SetParentEquipment();
        //                 break;
        //             }
        //             SetParentInventory();
        //             break;
        //         }
                
        //     }

        //     SetParentWorld()
        // }
        if (In((RectTransform)_originalParent)) {
            SetParentInventory();
        } else {
            SetParentWorld();
        }
    }

    private void SetParentInventory() {
        int closestIndex = 0;
        for(int i = 0; i < _originalParent.transform.childCount; i++) {
            if (Vector3.Distance(transform.position, _originalParent.GetChild(i).position) < 
                Vector3.Distance(transform.position, _originalParent.GetChild(closestIndex).position)) {
                    closestIndex = i;
            }
        }
        transform.parent = _originalParent;
        transform.SetSiblingIndex(closestIndex);
    }

    private void SetParentEquipment() {

    }

    private void SetParentWorld() {
        Injecting?.Invoke();
    }

    // private Transform GetNewParent() {
    //     Transform new_parent;
    //     return new_parent;
    // }
}
