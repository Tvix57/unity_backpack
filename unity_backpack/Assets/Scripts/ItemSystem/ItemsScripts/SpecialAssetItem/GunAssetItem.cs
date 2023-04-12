using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Item/Weapon")] 
public class GunAssetItem : AssetItem
{
    public uint Damage => _damage;
    public ConsumablesAssetItem.Type AmmoType => _ammo_type;
    [SerializeField] private uint _damage = 0;
    [SerializeField] private ConsumablesAssetItem.Type _ammo_type;
}
