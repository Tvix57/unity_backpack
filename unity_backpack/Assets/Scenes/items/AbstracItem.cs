using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstracItem : MonoBehaviour
{

    public string Name; ///find default
    public int Count;
    public readonly double Mass;

    [SerializeField] readonly private int Item_id;
    [SerializeField] readonly private bool Is_usable;
    [SerializeField] readonly private string icon_path;

    public AbstracItem(int id) {
        //read db by id and get property
        Name = "test";
        Item_id = id;
        Mass = 1;
        Count = 1;
        Is_usable = false;
    }
    public virtual void UseItem() {
        if (Is_usable) {
            // action();
            --Count;
        }
    }

    public bool StackItem(int input_id) {
        if (input_id == Item_id) {
            return true;
        } else {
            return false;
        }
    }
}
