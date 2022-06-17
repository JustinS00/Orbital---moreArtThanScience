using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public Terrain terrain;
    public CameraController cam;
    public Town town;

    public Vector2 spawnPos;

    private bool gamePaused = false;
    
    // Start is called before the first frame update
    void Start()
    {
        terrain.StartTerrainGeneration();
        player.spawnPos = spawnPos;
        player.Spawn();
        cam.moveTo(spawnPos);
        town.StartTerrainGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(SettingsMenu.instance.difficulty);
    }

    public void TogglePause() {
        gamePaused = !gamePaused;
        if (gamePaused) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }

    }

    public bool isGamePaused() {
        return gamePaused;
    }
}
