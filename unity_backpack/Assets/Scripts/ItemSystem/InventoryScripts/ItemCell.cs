using UnityEngine;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour
{
    public bool Status => _status;
    public AssetItem Item => _item;

    private bool _status;
    private AssetItem _item = null;
    private ItemShower _currentItem = null;

    public void Render(bool status) {
        _status = status;
        if (status) {
            transform.Find("Background").GetComponent<RawImage>().gameObject.SetActive(false);
        } else {
            transform.Find("Background").GetComponent<RawImage>().gameObject.SetActive(true);
        }
    }

    public void RenderItem() {

    }

    public void SetItem(ItemShower item) {
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
