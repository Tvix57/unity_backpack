using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Item/Armor")] 
public class ArmorAssetItem : AssetItem
{
    public uint Armor => _armor;
    [SerializeField] private uint _armor = 0;
}
