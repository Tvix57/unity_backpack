using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    public void Render(bool status) {
        if (status) {
            transform.Find("Background").GetComponent<RawImage>().gameObject.SetActive(false);
        } else {
            transform.Find("Background").GetComponent<RawImage>().gameObject.SetActive(true);
        }
    }
}
