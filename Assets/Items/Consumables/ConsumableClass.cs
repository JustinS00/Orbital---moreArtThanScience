using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    public int healthAdded;

    public override ItemClass GetItem() {return this;}

    public ConsumableClass GetConsumable() {return this;}

}
