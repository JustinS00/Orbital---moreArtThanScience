using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlocksCollection", menuName = "Blocks Collection")]
public class BlocksCollection : ScriptableObject
{   

    #region Normal Blocks
    public BlockClass stone;
    public BlockClass dirt;
    public BlockClass grass;
    public BlockClass log;
    public BlockClass leaves;
    #endregion

    #region Ores
    public BlockClass coal_ore;
    public BlockClass iron_ore;
    public BlockClass gold_ore;
    public BlockClass diamond_ore;
    #endregion

    public BlockClass bedrock;
}
