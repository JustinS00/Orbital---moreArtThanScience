using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boots Class", menuName = "Item/Equipment/Armour/Boots")]
public class BootsClass : ArmourClass{

    public Sprite boots_0_sideview;
    public Sprite boots_1_sideview;

    public override ItemClass GetItem() {return this;}

    public BootsClass GetBoots() {return this;}

}