using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass {

    public enum ItemType {block, tool};

    public enum ToolType {axe, pickaxe, sword};

    public ItemType itemType;
    public ToolType toolType;

    public BlockClass block;
    public ToolClass tool;

    public string name;
    public Sprite sprite;
    public bool isStackable;

    public ItemClass(BlockClass _block) {
        name = _block.blockName;
        sprite = _block.blockSprite;
        isStackable = true;
        itemType = ItemType.block;
    }

    public ItemClass(ToolClass _tool) {
        name = _tool.toolName;
        sprite = _tool.sprite;
        isStackable = false;
        toolType = _tool.toolType;
    }

    public ItemClass(GameObject _obj) {
        name = _obj.name;
        sprite = _obj.GetComponent<SpriteRenderer>().sprite;
        isStackable = true;
        itemType = ItemType.block;
    }

}
