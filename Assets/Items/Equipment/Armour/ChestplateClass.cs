using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chestplate Class", menuName = "Item/Equipment/Armour/Chestplate")]
public class ChestplateClass : ArmourClass{

    public Sprite chestplate1_sideview;
    public Sprite chestplate2_0_sideview;
    public Sprite chestplate2_1_sideview;

    public override ItemClass GetItem() {return this;}

    public ChestplateClass GetChestplate() {return this;}

}