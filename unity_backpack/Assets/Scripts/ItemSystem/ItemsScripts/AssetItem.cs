using System;
using UnityEngine;

public class AssetItem : ScriptableObject, IItem
{
    public int ID => _id;
    public string Name => _name;
    public Sprite Icon => _icon;
    public double Mass => _mass;
    public IItem.ItemType Type => _type;
    public IItem.ItemSlot Slot => _slot;
    public bool Stackable => _stackable;
    public uint Max_stack =>_max_stack;
    public uint Count {
        get { return _count; }
        set {  
            if (value <= _max_stack) {
                _count = value;
            }
        }
    }

    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private double _mass;
    [SerializeField] private IItem.ItemType _type;
    [SerializeField] private IItem.ItemSlot _slot;
    [SerializeField] private bool _stackable = false;
    [SerializeField] private uint _max_stack;
    [SerializeField] private uint _count;


    public bool Equals(int id) {
        return ID == id;
    }

}
