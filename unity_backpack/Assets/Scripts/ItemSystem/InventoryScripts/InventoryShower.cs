using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryShower : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
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
        //if (coord - on enventory)
        // transfor.parent = GetNewParent();
        // if (transfor.parent == )
        // SetParentInventory();
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

    }

    private Transform GetNewParent() {
        Transform new_parent;
        return new_parent;
    }
}
