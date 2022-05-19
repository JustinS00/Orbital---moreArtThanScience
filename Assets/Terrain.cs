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


    // Can do the same for cameracontroller
    public PlayerController player;
    public GameObject blockDrop;

    [Header("Blocks")]
    public BlocksCollection blocksCollection;

    [Header("World Settings")]
    public float seed;
    public int worldSize = 100;
    public int worldHeight = 128;
    private int heightAddition = 64;
    private int heightMulitplier = 5;
    public int dirtLayerHeight = 5;
    private int chunkSize = 16;

    [Header("Tree Settings")]
    public float treeChance = 0.03f;
    public int minTreeHeight = 6;
    public int maxTreeHeight = 10;

    [Header("Generation Settings")]
    public float surfaceValue = 0.25f;
    public float caveFreq = 0.03f;
    public float terrainFreq = 0.10f;
    public Texture2D noiseTexture;//2d array

    private List<Vector2> worldBlocks = new List<Vector2>();
    private List<GameObject> worldBlocksObject = new List<GameObject>();
    private List<BlockClass> worldBlockClasses = new List<BlockClass>();

    private int spawnX = 0;
    private int spawnY = 0;


    public GameObject[] worldChunks;

    private void Start() {
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

        seed = Random.Range(-10000,10000);
        spawnX = worldSize / 2;
        GenerateNoiseTexture();
        GenerateChunks();
        GenerateTerrain();

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
        player.spawnPos = new Vector2(spawnX, spawnY + 2);
        player.Spawn();

    }

    private void Update() {
        RefreshChunks();
    }

    void RefreshChunks() {
        for (int i = 0; i < worldChunks.Length; i ++) {
            worldChunks[i].SetActive(false);
        }
        int xLeft = Mathf.FloorToInt(player.transform.position.x - Camera.main.orthographicSize * 3f);
        int xRight = Mathf.CeilToInt(player.transform.position.x + Camera.main.orthographicSize * 3f);
        for (int i = xLeft; i <= xRight; i += chunkSize) {
            worldChunks[getChunkNo(i)].SetActive(true);
        }
    }

    private int getChunkNo(int x, int y) {
        return getChunkNo(x);
    }

     private int getChunkNo(int x) {
        return Mathf.FloorToInt(x / (float) chunkSize);
    }
    
    public void GenerateNoiseTexture() {
        noiseTexture = new Texture2D(worldSize, worldHeight);

        for (int x = 0; x < noiseTexture.width; x++) {
            for (int y = 0; y < noiseTexture.height; y++) {
                float value = Mathf.PerlinNoise((x + seed) * caveFreq, (y + seed) * caveFreq);
                noiseTexture.SetPixel(x, y, new Color(value, value, value));
            }
        }
        noiseTexture.Apply();
    }

    public void GenerateChunks() {
        int numChunks = Mathf.CeilToInt(worldSize / (float) chunkSize);
        worldChunks = new GameObject[numChunks];
        for (int i = 0; i < numChunks; i ++) {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            worldChunks[i] = newChunk;
            newChunk.transform.parent = this.transform;
        }
    }


    public void GenerateTerrain() {
        for (int i = 0; i < worldSize; i++) {
            float height = Mathf.PerlinNoise(i * terrainFreq, seed * terrainFreq) * heightMulitplier + heightAddition;
            for (int j = 0; j < height; j++) {
                BlockClass block; // change to BlockClass block
                if (j < height - dirtLayerHeight) {
                    block = blocksCollection.stone;
                } else if (j < height - 1) {
                    block = blocksCollection.dirt;
                } else {
                    block = blocksCollection.grass;
                }
                if (noiseTexture.GetPixel(i,j).r > surfaceValue) {
                    placeBlock(i, j, block);
                    if (block == blocksCollection.grass) {
                        float spawnTreeChance = Random.Range(0.0f, 1.0f);
                        if (spawnTreeChance < treeChance) {
                            generateTree(i, j + 1);
                        }
                    }
                }
            }
        }

        //worldBlocksMap.Apply();
    }

    public void generateTree(int x, int y) {
        int treeHeight = Random.Range(minTreeHeight, maxTreeHeight) + 1;
        for (int i = 0; i < treeHeight; i ++) {
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

    public bool canPlace(int x, int y) {
        return !worldBlocks.Contains(new Vector2(x, y));
    }

    public void placeBlock(int x, int y, BlockClass block) {//change sprite block to blockclass block
        //setting spawnPoint of player - should not be here
        if (x == spawnX & y > spawnY) {
            spawnY = y;
        }

        if (canPlace(x, y) && x >= 0 && x < worldSize && y >= 0 && y < worldHeight) {
            //lighting remove lighting if tile is placed
            /*
            RemoveLightSource(x,y);
            */
            //Normal Code
            GameObject newBlock = new GameObject();
            int chunkCoordinate = getChunkNo(x, y);
            newBlock.transform.parent = worldChunks[chunkCoordinate].transform;          
            newBlock.AddComponent<SpriteRenderer>();
            newBlock.AddComponent<BoxCollider2D>();
            newBlock.GetComponent<BoxCollider2D>().size = Vector2.one;
            newBlock.tag = "Ground";
            newBlock.GetComponent<SpriteRenderer>().sprite = block.blockSprite;
            newBlock.name = block.blockName;
            newBlock.transform.position = new Vector2(x + 0.5f, y + 0.5f);

            worldBlocks.Add(newBlock.transform.position - (Vector3.one * 0.5f));
            worldBlocksObject.Add(newBlock);
            worldBlockClasses.Add(block);
        }
    }

    public void destroyBlock(int x, int y) {
        if (worldBlocks.Contains(new Vector2(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldHeight) {
            Vector2 pos = new Vector2(x, y);
            GameObject obj = worldBlocksObject[worldBlocks.IndexOf(new Vector2(x, y))];
            BlockClass block = worldBlockClasses[worldBlocks.IndexOf(new Vector2(x, y))];
            Destroy(obj.gameObject);
            //worldBlocksMap.SetPixel(x,y, Color.white);
            //LightBlock(x, y, 1f, 0);
            GameObject newBlockDrop = Instantiate(blockDrop, new Vector2(x, y + 0.5f), Quaternion.identity);
            newBlockDrop.GetComponent<SpriteRenderer>().sprite = obj.GetComponent<SpriteRenderer>().sprite;

            ItemClass tileDropItem = new ItemClass(block);
            newBlockDrop.GetComponent<BlockDropCollider>().item = tileDropItem;
            
            worldBlocksObject.RemoveAt(worldBlocks.IndexOf(new Vector2(x, y)));
            worldBlockClasses.RemoveAt(worldBlocks.IndexOf(new Vector2(x, y)));
            worldBlocks.Remove(new Vector2(x,y));
            //worldBlocksMap.Apply();
        }
    }


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
}
