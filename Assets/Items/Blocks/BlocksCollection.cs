using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlocksCollection", menuName = "Blocks Collection")]
public class BlocksCollection : ScriptableObject
{   

    #region Normal Blocks
    public BlockClass stone;
    public BlockClass dirt;
    public BlockClass grass_block;
    public BlockClass log;
    public BlockClass leaves;
    #endregion

    #region Surface Blocks
    public BlockClass grass;
    public BlockClass mushroom_red;
    public BlockClass mushroom_brown;
    #endregion

    #region Ores
    public BlockClass coal_ore;
    public BlockClass iron_ore;
    public BlockClass gold_ore;
    public BlockClass diamond_ore;
    #endregion

    #region Decoration Blocks
    public BlockClass brick_red;
    public BlockClass brick_grey;
    #endregion
    
    #region Unbreakable Blocks
    public BlockClass bedrock;
    public BlockClass boundary;
    #endregion

}
