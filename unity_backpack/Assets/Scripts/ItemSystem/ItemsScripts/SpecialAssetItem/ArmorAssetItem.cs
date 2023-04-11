using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorAssetItem : AssetItem
{
    public uint Armor => _armor;
    [SerializeField] private uint _armor = 0;
}
