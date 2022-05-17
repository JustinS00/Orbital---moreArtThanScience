using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlocksCollection", menuName = "Blocks Collection")]
public class BlocksCollection : ScriptableObject
{
    public BlockClass stone;
    public BlockClass dirt;
    public BlockClass grass;
    public BlockClass log;
    public BlockClass leaves;

}
