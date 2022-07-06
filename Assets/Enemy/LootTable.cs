using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot {

    public ItemClass mobDrop;
    public int dropChance;

}

[CreateAssetMenu(fileName = "LootTable", menuName = "ScriptableObjects/LootTable")]
public class LootTable : ScriptableObject {
    public Loot[] loots;
    public GameObject itemDrop;


    public void generateLoot(int numberOfItemsToDrop, Vector2 position) {
        if (numberOfItemsToDrop == 0) return;

        int lootProbabilitySum = 100;
        
        while (numberOfItemsToDrop > 0) {
           
            foreach (Loot item in loots) {
                int randNum = Random.Range(0, lootProbabilitySum);
                if (randNum < item.dropChance) {
                    GameObject newItemDrop = Instantiate(itemDrop, position, Quaternion.identity);
                    newItemDrop.GetComponent<SpriteRenderer>().sprite = item.mobDrop.itemSprite;
                    newItemDrop.GetComponent<ItemDropCollider>().item = item.mobDrop;
                    newItemDrop.GetComponent<ItemDropCollider>().quantity = 1;
                    break;
                }
            }

            numberOfItemsToDrop--;
        }
        return;
    }

}