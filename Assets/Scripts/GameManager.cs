using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public PlayerController player;
    public Terrain terrain;
    public CameraController cam;
    public Town town;
    public Portal portal;
    private Portal terrainPortal;
    private Portal townPortal;

    public Vector2 spawnPos;

    private bool gamePaused = false;


    // Start is called before the first frame update
    void Awake() {
        terrain.StartTerrainGeneration();
        player.spawnPos = spawnPos;
        player.Spawn();
        cam.moveTo(spawnPos);
        town.StartTerrainGeneration();
        SpawnPortalTerrain();
        SpawnPortalTown();
    }

    void FixedUpdate() {
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(OptionsMenu.instance.difficulty);
        //Debug.Log(DayNightCycle.instance.isNight());
        //DayNightCycle.instance.DisplayTime();
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

    private void SpawnPortalTerrain() {
        Portal newPortal = Instantiate(portal);
        newPortal.transform.SetParent(terrain.transform, false);
        newPortal.SetEnableMessage("Press 'T' to travel to Town Area");
        newPortal.SetDisableMessage("Portal is disabled. To enable kill the Boss Monster");
        newPortal.SetLocation(new Vector2(50, 211));
        newPortal.transform.localPosition = spawnPos + new Vector2(0, 3);
        newPortal.TogglePortalOn();
        terrainPortal = newPortal;
    }

    private void SpawnPortalTown() {
        Portal newPortal = Instantiate(portal);
        newPortal.transform.SetParent(town.transform, false);
        newPortal.SetEnableMessage("Press 'T' to travel to Wilderness");
        newPortal.SetDisableMessage("Portal is disabled. To enable kill the Boss Monster");
        newPortal.SetLocation(spawnPos);
        newPortal.transform.localPosition = new Vector2(60, 13);
        newPortal.TogglePortalOn();
        townPortal = newPortal;
    }

    public void DisablePortals() {
        terrainPortal.TogglePortalOff();
        townPortal.TogglePortalOff();
    }

    public void EnablePortals() {
        terrainPortal.TogglePortalOn();
        townPortal.TogglePortalOn();
    }


}
