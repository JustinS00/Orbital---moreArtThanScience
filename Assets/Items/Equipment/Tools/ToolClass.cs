using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType {all, pickaxe, axe, shovel, none};

[CreateAssetMenu(fileName = "Tool Class", menuName = "Item/Equipment/Tool")]

public class ToolClass : EquipmentClass{

    public ToolType toolType;
    public ToolTier toolTier;
    public enum ToolTier {wooden, stone, iron, gold, diamond};
    public float miningSpeed;
    
    public override ItemClass GetItem() {return this;}

    public ToolClass GetTool() {return this;}
}


