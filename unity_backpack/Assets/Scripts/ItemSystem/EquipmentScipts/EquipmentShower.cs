using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentShower : InventoryShower {
    public IItem.ItemSlot SlotType => _slot;

    [SerializeField] private IItem.ItemSlot _slot;

        public void Render(IItem item) {
            _icon.sprite = item.Icon;
        }
}


