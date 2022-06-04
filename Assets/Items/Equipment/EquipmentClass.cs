using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EquipmentClass :  ItemClass
{
    public EquipmentType equipmentType;
    public enum EquipmentType {tool, armour, weapon};
    
    public int durability;
    public int durabilityRemaining;
    private bool isBroken;

    public int getTotalDurability() {
        return durability;
    }

    public int getCurrentDurability() {
        return durabilityRemaining;
    }

    public void setDurability (int dur) {
        durabilityRemaining = Mathf.Min(durability, dur);
        if (durabilityRemaining <= 0) {
            isBroken = true;
        }
    }

    public void reduceDurability(int reduction) {
        setDurability(durabilityRemaining - reduction);
    }

    public bool isEquipmentBroken() {
        return isBroken;
    }

}
