using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot {   
    public ItemClass item;
    public int quantity;

    
    public InventorySlot(ItemClass item, int quantity) {
        this.item = item;
        this.quantity = quantity;
    }

    public InventorySlot(InventorySlot slot) {
        this.item = slot.item;
        this.quantity = slot.quantity;
    }
    
    

}
