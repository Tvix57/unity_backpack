using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Item/Weapon")] 
public class GunAssetItem : AssetItem
{
    public uint Damage => _damage;
    [SerializeField] private uint _damage = 0;
}
