using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot {
    public ItemClass mobDrop;
    public int maxDropPerItem;
    public int dropChance;
}

[CreateAssetMenu(fileName = "LootTable", menuName = "ScriptableObjects/LootTable")]
public class LootTable : ScriptableObject {
    public Loot[] loots;
    public GameObject itemDrop;
    private int lootMaxProbablity = 100;

    public void GenerateLoot(Vector2 position) {
        foreach (Loot item in loots) {
            for (int i = 0; i < item.maxDropPerItem; i++) {
                int randNum = Random.Range(0, lootMaxProbablity);
                if (randNum < item.dropChance) {
                    GameObject newItemDrop = Instantiate(itemDrop, position, Quaternion.identity);
                    newItemDrop.GetComponent<SpriteRenderer>().sprite = item.mobDrop.itemSprite;
                    newItemDrop.GetComponent<ItemDropCollider>().item = item.mobDrop;
                    newItemDrop.GetComponent<ItemDropCollider>().quantity = 1;
                }
            }
        }
    }

}