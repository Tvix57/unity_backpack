using System;
using UnityEngine;

public interface IItem
{
    public enum ItemType {
        Armor, 
        Gun,
        Cons
    }
    int ID { get; }
    string  Name { get; }
    Sprite  Icon { get; }
    double  Mass { get; }
    ItemType Type { get; }
    bool    Stackable { get; }
    uint    Max_stack { get; }
    uint    Count { get; set; }
}
