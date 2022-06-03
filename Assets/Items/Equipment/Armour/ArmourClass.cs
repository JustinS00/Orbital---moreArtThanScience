using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ArmourClass : EquipmentClass{
    
    public ArmourType armourType;
    public enum ArmourType {helmet, chestplate, leggings, boots};
    public ArmourTier armourTier;
    public enum ArmourTier {leather, iron, gold, diamond};
    public int protectionValue;
    
    public override ItemClass GetItem() {return this;}

    public ArmourClass GetArmour() {return this;}

}
