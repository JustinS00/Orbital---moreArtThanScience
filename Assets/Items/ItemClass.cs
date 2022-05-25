using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemClass : ScriptableObject{
    
    public ItemType itemType;
    public enum ItemType {block, equipment, miscellaneous, consumable};
    public string itemName;
    public Sprite itemSprite;
    public bool isStackable;
    public int maxItemsPerStack;

    public abstract ItemClass GetItem();
 
}
