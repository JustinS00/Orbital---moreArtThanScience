using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Block Class", menuName = "Item/Block")]

public class BlockClass : ItemClass
{
    public bool isSolid = true;
    public bool isBreakable = true;
    public int hardness;
    public ToolType preferredTool;
    public enum ToolType {all, pickaxe, axe, shovel, none};
    
    public override ItemClass GetItem() {
        return this;
    }

    public BlockClass GetBlock() {
        return this;
    }

}
