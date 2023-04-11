using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAssetItem : MonoBehaviour
{
    public uint Damage => _damage;
    [SerializeField] private uint _damage = 0;
}
