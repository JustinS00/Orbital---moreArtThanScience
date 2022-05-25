using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordClass : WeaponClass
{

    public SwordTier swordTier;
    public enum SwordTier {wooden, stone, iron, gold, diamond};

    
    public override ItemClass GetItem() {return this;}

    public SwordClass GetSword() {return this;}

}
