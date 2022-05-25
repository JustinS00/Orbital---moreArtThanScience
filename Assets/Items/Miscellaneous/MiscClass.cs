using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Misc Class", menuName = "Item/Miscellaneous")]
public class MiscClass :  ItemClass
{
    public override ItemClass GetItem() {return this;}

    public MiscClass GetMisc() {return this;}
}
