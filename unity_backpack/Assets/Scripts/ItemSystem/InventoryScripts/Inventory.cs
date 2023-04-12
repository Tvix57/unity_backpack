using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<AssetItem> Items;
    [SerializeField] private InventoryShower _inventoryShower;
    [SerializeField] private InventoryCell _inventoryCell;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _containerBackground;
    [SerializeField] private Transform _draggingParent;
    [SerializeField] private uint _max_size = 30;
    [SerializeField] private uint _unlock_size = 15;
    [SerializeField] private uint _items_in_row = 5;

    public void Start() {
        SetSize();
    }

    public void OnEnable() {
        RenderItems(Items);
        RenderBack();
    }

    private void SetSize() {
        GameObject parentObject = GameObject.Find("Inventory");
        GridLayoutGroup[] gridLayouts = parentObject.GetComponentsInChildren<GridLayoutGroup>();
        float cell_width = ((parentObject.GetComponent<RectTransform>().rect.width - 10) / _items_in_row)-(gridLayouts[0].spacing.x);
        foreach (GridLayoutGroup gridLayout in gridLayouts) {
            gridLayout.cellSize = new Vector2(cell_width, cell_width);
        }
    }

    public void RenderItems(List<AssetItem> items) {
        foreach (Transform child in _container) {
            Destroy(child.gameObject);
        }
        items.ForEach(item => {
            var cell = Instantiate(_inventoryShower, _container);
            cell.Init(_draggingParent);
            cell.Render(item);
        });
    }

    public void RenderBack() {
        foreach (Transform child in _containerBackground) {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < _max_size; ++i) {
            var cell = Instantiate(_inventoryCell, _containerBackground);
            if (i < _unlock_size) {
                cell.Render(true);
            } else {
                cell.Render(false);
            }
        }
    }

    public void AddItem(AssetItem item) {
        if (Items.Count < _unlock_size) {
            Items.Add(item);
            RenderItems(Items);
        }
    }

    public void RemoveItem() {
        if (Items.Count != 0) {
            Items.RemoveAt(UnityEngine.Random.Range(0, Items.Count));
            RenderItems(Items);
        }
    }

    public void AddRandomItem() {

        // GameObject[] prefabs;
        // if (prefabs.Length != 0) {
        //     int randomIndex = UnityEngine.Random.Range(0, prefabs.Length);
        //     GameObject randomPrefab = Instantiate(prefabs[randomIndex]);
        // }

        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            // prefabs.Add(Resources.Load<GameObject>(AssetDatabase.GUIDToAssetPath(guid)));
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log(path);
        }
        // Debug.Log(prefabs.Length.ToString());
        // var new_item = new AssetItem();
        // AddItem(new_item);
    }

    public void AddAmmo() {
        // Object[] prefabs_ammo = Resources.LoadAll("Prefabs/AmmoPrefabs", typeof(GameObject));
        // for (int i = 0; i < prefabs_ammo.Length; ++i) {
        //     if (Items.Count < _unlock_size) {

                
        //         item.Count = item.Max_stack;
        //         AddItem(item);
        //     } else {
        //         break;
        //     }
        // }
    }

    public bool GetAmmo() {
        //find ammo and -1
        RenderItems(Items);
        return false;
    }
}
 
