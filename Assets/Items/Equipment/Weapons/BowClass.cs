using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow Class", menuName = "Item/Equipment/Weapon/Bow")]
public class BowClass : WeaponClass {

    public override ItemClass GetItem() {return this;}

    public BowClass GetBow() {return this;}
}
