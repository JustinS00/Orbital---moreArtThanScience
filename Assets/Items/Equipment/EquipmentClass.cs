using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EquipmentClass :  ItemClass
{
    public EquipmentType equipmentType;
    public enum EquipmentType {tool, armour, weapon};
    
    public int durability;
}
