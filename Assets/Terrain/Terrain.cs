using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO refactor code
public class Terrain : MonoBehaviour {

    /*
    [Header("Lighting")]
    public Texture2D worldBlocksMap;
    public Material lightShader;
    public float lightThreshold;
    public float lightRadius = 7f;
    List<Vector2Int> unlitBlocks = new List<Vector2Int>();
    */

    public GameManager gameManager;
    public GameObject itemDrop;
    public Portal portal;

    [Header("World Settings")]
    public float seed;
    public int worldSize = 500;
    public int worldHeight = 128;
    private int heightAddition = 64;
    private int heightMulitplier = 5;
    public int dirtLayerHeight = 5;
    private int chunkSize = 16;

    [Header("Generation Settings")]
    public float surfaceValue = 0.25f;
    public float caveFreq = 0.03f;
    public float terrainFreq = 0.10f;
    public Texture2D noiseTexture;//2d array

    [Header("Blocks")]
    public BlocksCollection blocksCollection;

    [Header("Surface Features Settings")]
    public float treeChance = 0.03f;
    public int minTreeHeight = 6;
    public int maxTreeHeight = 10;
    public float mushroomChance = 0.02f;
    public float grassChance = 0.03f;

    [Header("Ore Settings")]
    public int coalOreHeight = 128;
    public int ironOreHeight = 64;
    public int goldOreHeight = 32;
    public int diamondOreHeight = 16;
    public float coalRarity = 0.1f;
    public float ironRarity = 0.08f;
    public float goldRarity = 0.07f;
    public float diamondRarity = 0.06f;
    public float coalVeinSize = 0.76f;
    public float ironVeinSize = 0.8f;
    public float goldVeinSize = 0.85f;
    public float diamondVeinSize = 0.84f;
    /*Change to public to view map of ores spread*/
    private Texture2D coalTexture;
    private Texture2D ironTexture;
    private Texture2D goldTexture;
    private Texture2D diamondTexture;

    private List<Vector2> worldBlocks = new List<Vector2>();
    private List<GameObject> worldBlocksObject = new List<GameObject>();
    private List<BlockClass> worldBlockClasses = new List<BlockClass>();

    private int spawnX = 0;
    private int spawnY = 0;

    public GameObject[] worldChunks;
    private int numChunks;

    #region Initialisation
    /* For Visualisation
    private void OnValidate() {
        if (noiseTexture == null) {
            noiseTexture = new Texture2D(worldSize, worldHeight);
            coalTexture = new Texture2D(worldSize, worldHeight);
            ironTexture = new Texture2D(worldSize, worldHeight);
            goldTexture = new Texture2D(worldSize, worldHeight);    
            diamondTexture = new Texture2D(worldSize, worldHeight);
        }
        GenerateNoiseTexture(noiseTexture, caveFreq, surfaceValue);
        GenerateNoiseTexture(coalTexture, coalRarity, (float) coalVeinSize);
        GenerateNoiseTexture(ironTexture, ironRarity, (float) ironVeinSize);
        GenerateNoiseTexture(goldTexture, goldRarity, (float) goldVeinSize);
        GenerateNoiseTexture(diamondTexture, diamondRarity, (float) diamondVeinSize);   
    }
    */
    public void StartTerrainGeneration() {
        //lighting
        /*
        worldBlocksMap = new Texture2D(worldSize, worldHeight);
        worldBlocksMap.filterMode = FilterMode.Point;
        lightShader.SetTexture("_ShadowTex", worldBlocksMap);

        for (int x = 0; x < worldSize; x++) {
            for (int y = 0; y < worldHeight; y++) {
                worldBlocksMap.SetPixel(x,y, Color.white);
            }
        }
        worldBlocksMap.Apply();
        */

        seed = Random.Range(-100000, 100000);
        spawnX = worldSize / 2;
        if (noiseTexture == null) {
            noiseTexture = new Texture2D(worldSize, worldHeight);
            coalTexture = new Texture2D(worldSize, worldHeight);
            ironTexture = new Texture2D(worldSize, worldHeight);
            goldTexture = new Texture2D(worldSize, worldHeight);
            diamondTexture = new Texture2D(worldSize, worldHeight);
        }

        GenerateNoiseTexture(noiseTexture, caveFreq, surfaceValue);
        GenerateNoiseTexture(coalTexture, coalRarity, (float) coalVeinSize);
        GenerateNoiseTexture(ironTexture, ironRarity, (float) ironVeinSize);
        GenerateNoiseTexture(goldTexture, goldRarity, (float) goldVeinSize);
        GenerateNoiseTexture(diamondTexture, diamondRarity, (float) diamondVeinSize);
        //GenerateChunks();
        GenerateTerrain();
        SpawnPortal();

        /*
        for (int x = 0; x < worldSize; x++) {
            for (int y = 0; y < worldHeight; y++) {
                if (worldBlocksMap.GetPixel(x,y) == Color.white) {
                    LightBlock(x, y, 1f, 0);
                }
            }
        }
        worldBlocksMap.Apply();
        */
        gameManager.spawnPos = new Vector2(spawnX, spawnY + 2);

    }

    public void GenerateNoiseTexture(Texture2D noiseTexture, float freq, float limit) {

        for (int x = 0; x < noiseTexture.width; x++) {
            for (int y = 0; y < noiseTexture.height; y++) {
                float value = Mathf.PerlinNoise((x + seed) * freq, (y + seed) * freq);
                if (value > limit) {
                    noiseTexture.SetPixel(x, y, Color.white);
                } else {
                    noiseTexture.SetPixel(x, y, Color.black);
                }
            }
        }
        noiseTexture.Apply();
    }

    /**
    public void GenerateChunks() {
        numChunks = Mathf.CeilToInt(worldSize / (float) chunkSize);
        worldChunks = new GameObject[numChunks];
        for (int i = 0; i < numChunks; i++) {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            worldChunks[i] = newChunk;
            newChunk.transform.parent = this.transform;
            newChunk.SetActive(false);
        }
    }
    **/

    public void GenerateTerrain() {
        for (int i = 0; i < worldSize; i++) {
            float height = Mathf.PerlinNoise(i * terrainFreq, seed * terrainFreq) * heightMulitplier + heightAddition;
            for (int j = 0; j < height; j++) {
                if (j == 0) {
                    placeBlock(i, j, blocksCollection.bedrock);
                } else {
                    BlockClass block;
                    if (j < height - dirtLayerHeight) {
                        if (coalTexture.GetPixel(i, j) == Color.white && j < coalOreHeight) {
                            block = blocksCollection.coal_ore;
                        } else if (ironTexture.GetPixel(i, j) == Color.white && j < ironOreHeight) {
                            block = blocksCollection.iron_ore;
                        } else if (goldTexture.GetPixel(i, j) == Color.white && j < goldOreHeight) {
                            block = blocksCollection.gold_ore;
                        } else if (diamondTexture.GetPixel(i, j) == Color.white && j < diamondOreHeight) {
                            block = blocksCollection.diamond_ore;
                        } else {
                            block = blocksCollection.stone;
                        }
                    } else if (j < height - 1) {
                        block = blocksCollection.dirt;
                    } else {
                        block = blocksCollection.grass_block;
                    }
                    placeBackGroundBlock(i, j, block);
                    if (noiseTexture.GetPixel(i, j) == Color.white) {
                        placeBlock(i, j, block);
                        if (block == blocksCollection.grass_block) {
                            float spawnTreeChance = Random.Range(0.0f, 1.0f);
                            float spawnMushroomChance = Random.Range(0.0f, 1.0f);
                            float spawnGrassChance = Random.Range(0.0f, 1.0f);
                            if (spawnTreeChance < treeChance) {
                                generateTree(i, j + 1);
                            } else if (spawnMushroomChance < mushroomChance) {
                                float redOrBrown = Random.Range(0.0f, 1.0f);
                                if (redOrBrown < 0.5f) {
                                    placeBlock(i, j + 1, blocksCollection.mushroom_brown);
                                } else {
                                    placeBlock(i, j + 1, blocksCollection.mushroom_red);
                                }
                            } else if (spawnGrassChance < grassChance) {
                                placeBlock(i, j + 1, blocksCollection.grass);
                            }
                        }
                    }
                }
            }
        }
        //worldBlocksMap.Apply();
    }

    private void SpawnPortal() {
        Portal newPortal = Instantiate(portal);
        newPortal.transform.parent = this.transform;
        newPortal.SetMessage("Press 'T' to travel to Town Area");
        newPortal.SetLocation(new Vector2(50, 211));
        newPortal.transform.localPosition = new Vector2(spawnX, spawnY);
    }

    #endregion

    #region Update
    
    /**
    private void Update() {
        RefreshChunks();
    }

    void RefreshChunks() {
        for (int i = 0; i < worldChunks.Length; i++) {
            worldChunks[i].SetActive(false);
        }
        int xLeft = Mathf.FloorToInt(gameManager.player.transform.position.x - Camera.main.orthographicSize * 4f);
        int xRight = Mathf.CeilToInt(gameManager.player.transform.position.x + Camera.main.orthographicSize * 4f);
        for (int i = xLeft; i <= xRight; i += chunkSize) {
            int chunkNo = getChunkNo(i);
            if (chunkNo >= numChunks) {
                chunkNo = numChunks - 1;
            } else if (chunkNo < 0) {
                chunkNo = 0;
            }
            worldChunks[chunkNo].SetActive(true);
        }
    }

    private int getChunkNo(int x, int y) {
        return getChunkNo(x);
    }

    private int getChunkNo(int x) {
        return Mathf.FloorToInt(x / (float) chunkSize);
    }
    **/
    #endregion

    #region Structures
    public void generateTree(int x, int y) {
        int treeHeight = Random.Range(minTreeHeight, maxTreeHeight) + 1;
        for (int i = 0; i < treeHeight; i++) {
            placeBlock(x, y + i, blocksCollection.log);
        }
        for (int i = -2; i < 3; i++) {
            for (int j = treeHeight; j < treeHeight + 2; j++) {
                placeBlock(x + i, y + j, blocksCollection.leaves);
            }
        }
        for (int i = -1; i < 2; i++) {
            for (int j = treeHeight + 2; j < treeHeight + 3; j++) {
                placeBlock(x + i, y + j, blocksCollection.leaves);
            }
        }
    }
    #endregion

    #region Functions
    public bool canPlace(int x, int y) {
        if (x >= 0 && x < worldSize && y >= 0 && y < worldHeight)
            return !worldBlocks.Contains(new Vector2(x, y));
        return false;
    }

    public BlockClass GetBlock(int x, int y) {
        if (worldBlocks.Contains(new Vector2(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldHeight) {
            Vector2 pos = new Vector2(x, y);
            BlockClass block = worldBlockClasses[worldBlocks.IndexOf(new Vector2(x, y))];
            return block;
        }
        return null;
    }

    public void placeBlock(int x, int y, BlockClass block) {
        if (x == spawnX & y > spawnY) {
            spawnY = y;
        }

        if (canPlace(x, y)) {
            //lighting remove lighting if tile is placed
            /*
            RemoveLightSource(x,y);
            */
            //Normal Code
            GameObject newBlock = new GameObject();
            //int chunkCoordinate = getChunkNo(x, y);
            //newBlock.transform.parent = worldChunks[chunkCoordinate].transform;
            newBlock.transform.parent = this.transform;
            newBlock.AddComponent<SpriteRenderer>();
            if (block.isSolid) {
                newBlock.AddComponent<BoxCollider2D>();
                newBlock.GetComponent<BoxCollider2D>().size = Vector2.one;
            }
            newBlock.tag = "Ground";
            newBlock.GetComponent<SpriteRenderer>().sprite = block.itemSprite;
            newBlock.name = block.itemName;
            newBlock.transform.position = new Vector2(x + 0.5f, y + 0.5f);

            worldBlocks.Add(newBlock.transform.position - (Vector3.one * 0.5f));
            worldBlocksObject.Add(newBlock);
            worldBlockClasses.Add(block);
        }
    }

    private void placeBackGroundBlock(int x, int y, BlockClass block) {
        GameObject newBlock = new GameObject();
        //int chunkCoordinate = getChunkNo(x, y);
        //newBlock.transform.parent = worldChunks[chunkCoordinate].transform;
        newBlock.transform.parent = this.transform;
        newBlock.AddComponent<SpriteRenderer>();
        Sprite sprite = blocksCollection.stone.itemSprite;
        string name = "stone_background";
        if (block.itemName == "grass_block") {
            sprite = blocksCollection.grass_block.itemSprite;
            name = "grassblock_background";
        } else if (block.itemName == "dirt") {
            sprite = blocksCollection.dirt.itemSprite;
            name = "dirt_background";
        }
        newBlock.name = name;
        newBlock.GetComponent<SpriteRenderer>().sprite = sprite;
        newBlock.GetComponent<SpriteRenderer>().sortingOrder = -5;
        newBlock.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        newBlock.transform.position = new Vector2(x + 0.5f, y + 0.5f);
    }
    
    
    private bool blockIsSurfaceBlock(int x, int y) {
        BlockClass block = GetBlock(x, y);
        if (block) {
            if (block.itemName == "grass" || block.itemName.Contains("mushroom")) {
                return true;
            }
        }
        return false;
    }

    public void mineBlock(int x, int y, bool isPreferredTool) {
        if (worldBlocks.Contains(new Vector2(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldHeight) {
            Vector2 pos = new Vector2(x, y);
            GameObject obj = worldBlocksObject[worldBlocks.IndexOf(new Vector2(x, y))];
            BlockClass block = worldBlockClasses[worldBlocks.IndexOf(new Vector2(x, y))];
            
            if (blockIsSurfaceBlock(x, y + 1))
                mineBlock(x, y + 1, true);
            //worldBlocksMap.SetPixel(x,y, Color.white);
            //LightBlock(x, y, 1f, 0);
            if (isPreferredTool) {
                GameObject newBlockDrop = Instantiate(itemDrop, new Vector2(x, y + 0.5f), Quaternion.identity);
                newBlockDrop.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
                newBlockDrop.GetComponent<ItemDropCollider>().item = block;
                newBlockDrop.GetComponent<ItemDropCollider>().quantity = 1;
            }
            Destroy(obj.gameObject);
            worldBlocksObject.RemoveAt(worldBlocks.IndexOf(new Vector2(x, y)));
            worldBlockClasses.RemoveAt(worldBlocks.IndexOf(new Vector2(x, y)));
            worldBlocks.Remove(new Vector2(x, y));
            //worldBlocksMap.Apply();
        }
    }

    public void destroyBlock(int x, int y) {
        if (worldBlocks.Contains(new Vector2(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldHeight) {
            Vector2 pos = new Vector2(x, y);
            GameObject obj = worldBlocksObject[worldBlocks.IndexOf(new Vector2(x, y))];
            BlockClass block = worldBlockClasses[worldBlocks.IndexOf(new Vector2(x, y))];
            //worldBlocksMap.SetPixel(x,y, Color.white);
            //LightBlock(x, y, 1f, 0);
            GameObject newBlockDrop = Instantiate(itemDrop, new Vector2(x, y + 0.5f), Quaternion.identity);
            newBlockDrop.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;

            newBlockDrop.GetComponent<ItemDropCollider>().item = block;
            newBlockDrop.GetComponent<ItemDropCollider>().quantity = 1;
            Destroy(obj.gameObject);
            worldBlocksObject.RemoveAt(worldBlocks.IndexOf(new Vector2(x, y)));
            worldBlockClasses.RemoveAt(worldBlocks.IndexOf(new Vector2(x, y)));
            worldBlocks.Remove(new Vector2(x, y));
            //worldBlocksMap.Apply();
        }
    }
    #endregion

    #region Lightings
    //Lighting (Breaking game rn)
    /*
    void LightBlock(int x, int y, float intensity, int iteration) {
        if (iteration < lightRadius) {
            worldBlocksMap.SetPixel(x, y, Color.white * intensity);
            for (int nx = x - 1; nx < x + 2; nx++) {
                for (int ny = y - 1; ny < y + 2; ny++) {
                    if (nx != x || ny != y) {
                        float dist = Vector2.Distance(new Vector2(x,y), new Vector2(nx, ny));
                        float targetIntensity = Mathf.Pow(0.7f, dist) * intensity;
                        if (worldBlocksMap.GetPixel(nx, ny) != null) {
                            if (worldBlocksMap.GetPixel(nx, ny).r < targetIntensity) {
                                LightBlock(nx, ny, targetIntensity, iteration + 1);
                            }
                        }
                    }
                }
            }
            worldBlocksMap.Apply();
        }
    }

    void RemoveLightSource(int x, int y) {
        unlitBlocks.Clear();
        UnLightBlock(x, y, x, y);

        List<Vector2Int> toRelight = new List<Vector2Int>();
        foreach(Vector2Int block in unlitBlocks) {
            for (int nx = block.x - 1; nx < block.x + 2; nx ++) {
                for (int ny = block.y - 1; ny < block.y + 2; ny ++) {
                    if (worldBlocksMap.GetPixel(nx, ny) != null) {
                        if (worldBlocksMap.GetPixel(nx, ny).r > worldBlocksMap.GetPixel(block.x, block.y).r) {
                            if (!toRelight.Contains(new Vector2Int(nx,ny))) {
                                toRelight.Add(new Vector2Int(nx, ny));
                            }
                        }
                    }
                }
            }
        }
        foreach(Vector2Int source in toRelight) {
            LightBlock(source.x, source.y, worldBlocksMap.GetPixel(source.x, source.y).r, 0);
        }
        worldBlocksMap.Apply();
    }

    void UnLightBlock(int x, int y, int ix, int iy) {
        if (Mathf.Abs(x - ix) >= lightRadius || Mathf.Abs(y - iy) >= lightRadius || unlitBlocks.Contains(new Vector2Int(x, y))) {
            return;
        }

        for(int nx = x - 1; nx < x + 2; nx++){
            for (int ny = y - 1; ny < y + 2; ny++) {
                if (nx !=x || ny != y) {
                    if (worldBlocksMap.GetPixel(nx, ny) != null) {
                        if (worldBlocksMap.GetPixel(nx, ny).r < worldBlocksMap.GetPixel(x,y).r) {
                            UnLightBlock(nx, ny, ix, iy);
                        }
                    }
                }
            }
        }

        worldBlocksMap.SetPixel(x, y, Color.black);
        unlitBlocks.Add(new Vector2Int(x,y));
    }
    */
    #endregion
}
