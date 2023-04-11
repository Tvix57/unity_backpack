using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")] 
public class AssetItem : ScriptableObject, IItem
{
    public string Name => _name;
    public Sprite Icon => _icon;
    public double Mass => _mass;
    public bool Stackable {
        get { return _stackable; }
        set { 
            if (_stackable != value && !value) {
                _count = 1;
            }
            _stackable = value;
        }
    }
    public uint Max_stack =>_max_stack;
    public uint Count {
        get { return _count; }
        set {  
            if (value <= _max_stack) {
                _count = value;
            }
        }
    }

    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private double _mass;
    [SerializeField] private bool _stackable = false;
    [SerializeField] private uint _max_stack;
    [SerializeField] private uint _count = 1;
}
