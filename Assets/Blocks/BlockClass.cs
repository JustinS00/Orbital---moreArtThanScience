using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newBlockClass", menuName = "Block Class")]
public class BlockClass : ScriptableObject
{
    public string blockName;
    public Sprite blockSprite;
    public bool isSolid = true;

}
