using System;
using UnityEngine;

public interface IItem
{
    // int         Id { get; }
    string  Name { get; }
    Sprite  Icon { get; }
    // int         Count { get; set; }
    // double      Mass { get; set; }
    // bool        Is_usable { get; }
    // bool        Is_stackble { get; }
}
