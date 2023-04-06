using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCons : AbstracItem
{
    public enum Cons_type {
        Bullet,
        Food
    }
    public readonly Cons_type Item_type;

    public ItemCons(int id) : base (id) {
        //read db by id and get own property
        Item_type = Cons_type.Bullet;
    }
}
