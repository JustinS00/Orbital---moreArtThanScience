using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Helmet Class", menuName = "Item/Equipment/Armour/Helmet")]
public class HelmetClass : ArmourClass{

    public Sprite helmet_sideview;
    
    public override ItemClass GetItem() {return this;}

    public HelmetClass GetHelmet() {return this;}

}
