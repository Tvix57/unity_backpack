using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGun : AbstracItem
{
    public readonly int Damage_value;

    public ItemGun(int id) : base (id) {
        //read db by id and get own property
        Damage_value = 10;
    }
}
