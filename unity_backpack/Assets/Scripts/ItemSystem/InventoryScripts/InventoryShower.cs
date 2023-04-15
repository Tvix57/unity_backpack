using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class InventoryShower : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    enum DropTo {
        Inventory,
        Equpment,
        World
    }

    public event Action InInventory;
    public event Action Drop;
    public event Action Eqip;

    [SerializeField] private Text _nameField;
    [SerializeField] protected Image _icon;
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
        switch (WhereToDrop(eventData)) {
            case DropTo.Inventory: { InInventory?.Invoke(); break; }
            case DropTo.Equpment: { Eqip?.Invoke(); break; }
            case DropTo.World: { Drop?.Invoke(); break; }
        }
    }
    
    private DropTo WhereToDrop(PointerEventData eventData) {
        DropTo way = DropTo.World;
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        if (results.Count > 0) {
            foreach(RaycastResult result in results) {
                if (result.gameObject.GetComponent<Inventory>() != null) {
                    way = DropTo.Inventory;
                    break;
                }
                if (result.gameObject.GetComponent<Equipment>() != null) {
                    way = DropTo.Equpment;
                    break;
                }
            }
        }
        return way;
    }
}
