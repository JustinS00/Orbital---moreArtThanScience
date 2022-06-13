using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{

    public GameManager gameManager;
    
    [Header("Town Settings")]
    private int townSize = 100;
    private int dirtLayerHeight = 2;
    private int townHeight = 200;
    private int terrainHeight = 10;

    [Header("Blocks")]
    public BlocksCollection blocksCollection; 
    private List<GameObject> worldBlocksObject = new List<GameObject>();

    #region Initialisation

    private GameObject terrain;


    public void StartTerrainGeneration() {
        terrain = new GameObject();
        terrain.transform.parent = this.transform;
        terrain.name = "Town";
        GenerateFlatTerrain();
        GenerateStructures();
        SpawnNPCs();
    }

    private void GenerateFlatTerrain() {
        for (int i = 0; i < townSize; i++) {
            for (int j = 0; j < terrainHeight; j++) {
                if (j == 0) {
                    placeUnbreakableBlock(i, j + townHeight, blocksCollection.bedrock);
                } else {
                    BlockClass block;
                    if (j < terrainHeight - dirtLayerHeight) {
                        block = blocksCollection.stone;
                    } else if (j < terrainHeight - 1) {
                        block = blocksCollection.dirt;
                    } else {
                        block = blocksCollection.grass_block;
                    } 
                    placeUnbreakableBlock(i, j + townHeight, block);
                }
            }
        }
    }

    private void placeUnbreakableBlock(int x, int y, BlockClass block) {
        GameObject newBlock = new GameObject();       
        newBlock.AddComponent<SpriteRenderer>();

        if(block.isSolid) {
            newBlock.AddComponent<BoxCollider2D>();
            newBlock.GetComponent<BoxCollider2D>().size = Vector2.one;
        }
        newBlock.tag = "Ground";
        newBlock.GetComponent<SpriteRenderer>().sprite = block.itemSprite;
        newBlock.name = block.itemName;
        newBlock.transform.position = new Vector2(x + 0.5f, y + 0.5f);
        newBlock.transform.parent = terrain.transform;
    }

    #endregion

    #region Structures
    public void GenerateStructures() {

    }
    #endregion

    private void SpawnNPCs() {

    }

    #region Update
    private void Update() {

    }
    #endregion


}
