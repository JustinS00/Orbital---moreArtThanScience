using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot {   
    public ItemClass item;
    public Vector2Int position;
    public int quantity;

    
    public InventorySlot(ItemClass item, Vector2Int position, int quantity) {
        this.item = item;
        this.position = position;
        this.quantity = quantity;
    }
    
    

}
