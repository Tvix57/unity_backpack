using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Item/Consumables")] 
public class ConsumablesAssetItem : AssetItem
{
    public enum Type {
        Gun_bullet,
        Pistol_bullet,
        Food
    }
    public Type ItemType => _item_type;
    [SerializeField] private Type _item_type;
}
