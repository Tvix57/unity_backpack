using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryShower : MonoBehaviour
{
    [SerializeField] private Text _nameField;
    [SerializeField] private Image _icon;
    [SerializeField] private int _count;

    public void Render(IItem item) {
        _nameField.text = item.Name;
        _icon.sprite = item.Icon;
    }
}
