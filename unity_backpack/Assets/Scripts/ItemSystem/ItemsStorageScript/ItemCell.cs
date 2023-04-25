using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemCell : MonoBehaviour
{
    public bool Status => _status;
    public AssetItem Item => _item;

    private bool _status;
    private AssetItem _item = null;
    private ItemShower _itemShower = null;

    public void Render(bool status) {
        _status = status;
        if (status) {
            transform.Find("Background").GetComponent<RawImage>().gameObject.SetActive(false);
        } else {
            transform.Find("Background").GetComponent<RawImage>().gameObject.SetActive(true);
        }
    }

    public void SetItem(AssetItem item, ItemShower itemShower) {
        RemoveItem();
        _item = item;
        _itemShower = itemShower;
        itemShower.transform.parent = transform;
        itemShower.transform.position = transform.position;
    }
    
    public void RemoveItem() {
        if (_itemShower != null) {
            Destroy(_itemShower.gameObject);
            _itemShower = null;
        }
        _item = null;
    }
}
