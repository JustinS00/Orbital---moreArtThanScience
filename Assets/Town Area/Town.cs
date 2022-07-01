using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{

    public GameManager gameManager;
    public Portal portal;
    
    [Header("Town Settings")]
    private int townSize = 100;
    private int dirtLayerHeight = 2;
    private int townHeight = 200;
    private int terrainHeight = 10;

    private int BOUNDARY_X_MIN = 20;
    private int BOUNDARY_X_MAX = 80;
    private int BOUNDARY_HEIGHT = 10;

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
        GenerateBorder();
        GenerateStructures();
        SpawnNPCs();
        SpawnPortal();
    }

    private void SpawnPortal() {
        Portal newPortal = Instantiate(portal);
        newPortal.transform.SetParent(this.transform, false);
        newPortal.SetMessage("Press 'T' to travel to Wildeness");
        newPortal.SetLocation(new Vector2(250, 100));
        newPortal.transform.localPosition = new Vector2(60, 13);
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

    private void GenerateBorder() {
        for (int x = 0; x <= BOUNDARY_HEIGHT; x++) {
            placeUnbreakableBlock(BOUNDARY_X_MIN, x +  terrainHeight + townHeight,  blocksCollection.boundary);
            placeUnbreakableBlock(BOUNDARY_X_MAX, x +  terrainHeight + townHeight,  blocksCollection.boundary);
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
