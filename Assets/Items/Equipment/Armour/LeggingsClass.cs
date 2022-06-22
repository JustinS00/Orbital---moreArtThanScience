using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leggings Class", menuName = "Item/Equipment/Armour/Leggings")]
public class LeggingsClass : ArmourClass{


    public Sprite leggings_0_sideview;
    public Sprite leggings_1_sideview;

    public override ItemClass GetItem() {return this;}

    public LeggingsClass GetLeggings() {return this;}

}