using System;
using UnityEngine;

public interface IItem
{
    string  Name { get; }
    Sprite  Icon { get; }
    double  Mass { get; }
    bool    Stackable { get; set; }
    uint    Max_stack { get; }
    uint    Count { get; set; }
}
