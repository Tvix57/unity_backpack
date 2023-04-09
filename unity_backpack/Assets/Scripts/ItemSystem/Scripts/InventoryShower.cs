using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryShower : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private Text _nameField;
    [SerializeField] private Image _icon;
    [SerializeField] private int _count;

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
    }

    public void OnBeginDrag(PointerEventData eventData) {
        transform.parent = _draggingParent;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }
    
    public void OnEndDrag(PointerEventData eventData) {
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
}
