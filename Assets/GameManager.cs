using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public Terrain terrain;
    public CameraController cam;

    public Vector2 spawnPos;
    
    // Start is called before the first frame update
    void Start()
    {
        terrain.StartTerrainGeneration();
        player.spawnPos = spawnPos;
        player.Spawn();
        cam.moveTo(spawnPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
