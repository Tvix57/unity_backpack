using System;
using UnityEngine;

public interface IItem
{
    public enum ItemType {
        Armor, 
        Weapon,
        Consumables
    }
    public enum ItemSlot {
        Head, 
        Body,
        Hands,
        Trinket,
        NoSlot
    }
    
    int ID { get; }
    string  Name { get; }
    Sprite  Icon { get; }
    double  Mass { get; }
    ItemType Type { get; }
    ItemSlot Slot { get; }
    // event Action UpdateShower { get; }
    bool    Stackable { get; }
    uint    Max_stack { get; }
    uint    Count { get; set; }
}
