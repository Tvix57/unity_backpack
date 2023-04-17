using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    private InventoryShower _currentItem = null;
    public bool Status => _status;
    private bool _status;
    public void Render(bool status) {
        _status = status;
        if (status) {
            transform.Find("Background").GetComponent<RawImage>().gameObject.SetActive(false);
        } else {
            transform.Find("Background").GetComponent<RawImage>().gameObject.SetActive(true);
        }
    }

    public void SetItem(InventoryShower item) {
        RemoveItem();
        _currentItem = item;
        item.transform.parent = transform;
        item.transform.position = transform.position;
    }
    
    public void RemoveItem() {
        if (_currentItem != null) {
            Destroy(_currentItem.gameObject);
            _currentItem = null;
        }
    }


}
