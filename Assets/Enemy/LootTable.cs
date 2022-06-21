using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot {

    public GameObject loot;
    public MobDropsClass mobDrop;
    public int dropChance;

}

[CreateAssetMenu(fileName = "LootTable", menuName = "ScriptableObjects/LootTable")]
public class LootTable : ScriptableObject {
    public Loot[] loots;
    public GameObject itemDrop;

    public void generateLoot(int numberOfItemsToDrop, Vector2 position) {
        if (numberOfItemsToDrop == 0) return;

        int lootProbabilitySum = 0;
        foreach (Loot loot in loots) {
            lootProbabilitySum += loot.dropChance;
        }

        while (numberOfItemsToDrop > 0) {
            int randNum = Random.Range(0, lootProbabilitySum);
            foreach (Loot item in loots) {
                if (randNum < item.dropChance) {
                    if (item != null) {
                        GameObject newItemDrop = Instantiate(itemDrop, position, Quaternion.identity);
                        newItemDrop.GetComponent<SpriteRenderer>().sprite = item.loot.GetComponent<SpriteRenderer>().sprite;
                        newItemDrop.GetComponent<ItemDropCollider>().item = item.mobDrop;
                        newItemDrop.GetComponent<ItemDropCollider>().quantity = 1;
                        break;
                    }

                    break;
                }
            }

            numberOfItemsToDrop--;
        }
        return;
    }

}