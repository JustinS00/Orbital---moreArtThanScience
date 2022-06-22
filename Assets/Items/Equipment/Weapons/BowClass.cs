using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow Class", menuName = "Item/Equipment/Weapon/Bow")]
public class BowClass : WeaponClass {
    // fire rate in arrows/min
    public float fireRate;
    public override ItemClass GetItem() { return this; }

    public BowClass GetBow() { return this; }
}
