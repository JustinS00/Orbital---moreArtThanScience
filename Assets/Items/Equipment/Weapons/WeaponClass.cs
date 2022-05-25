using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Class", menuName = "Item/Equipment/Weapon")]
public abstract class WeaponClass : EquipmentClass{

    public WeaponType weaponType;
    public enum WeaponType {melee, ranged};

    public float damage;

}
