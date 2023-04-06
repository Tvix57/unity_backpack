using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemArmor : AbstracItem
{
    public enum Armor_slot {
        Head,
        Body,
        Legs
    }

    public readonly int Armor_value;
    public readonly Armor_slot Slot;

    public ItemArmor(int id) : base (id) {
        //read db by id and get own property
        Armor_value = 10;
        Slot = Armor_slot.Body;
    }
}
