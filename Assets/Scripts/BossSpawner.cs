using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour {

    [SerializeField] private GameManager gameManager;

    [SerializeField] private EntityCollection entityCollection;
    private GameObject[] entityPrefabs;

    private Vector3 spawnLocation;
    private float randX;
    private string currentTag;

    private GameObject player;

    private int clothBossSpawnDay = Int32.MaxValue;

    enum BossType {
        slimeking,
        clothBoss
    }

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        entityPrefabs = entityCollection.prefabs;
        clothBossSpawnDay = 10;
    }

    void Update() {
        int dayNo = DayNightCycle.instance.GetDayNo();
        if (MobStats.instance.kills[(int) MobType.slimeking] == 0) {
            if (dayNo >= 10 && dayNo % 5 == 0 && GameObject.FindGameObjectsWithTag("Boss").Length == 0) {
                int slimekingIndex = (int) BossType.slimeking;
                GameObject slimeking = entityPrefabs[slimekingIndex];

                spawnLocation = new Vector3(player.transform.position.x + randX, player.transform.position.y + 10, 0);
                while (Physics2D.OverlapCircle(spawnLocation, 1.0f) != null) {
                    spawnLocation = new Vector3(player.transform.position.x + randX, player.transform.position.y + 10, 0);
                }

                if (player.transform.position.y <= 128) {
                    GameObject newEntity = Instantiate(slimeking, spawnLocation, Quaternion.identity);
                    clothBossSpawnDay += 5;
                }
            }
        }

        if (MobStats.instance.kills[(int) MobType.clothboss] == 0) {
            if (dayNo >= clothBossSpawnDay && dayNo % 5 == 0 && GameObject.FindGameObjectsWithTag("Boss").Length == 0) {
                int clothBossIndex = (int) BossType.clothBoss;
                GameObject clothBoss = entityPrefabs[clothBossIndex];

                spawnLocation = new Vector3(player.transform.position.x + randX, player.transform.position.y + 10, 0);
                while (Physics2D.OverlapCircle(spawnLocation, 1.0f) != null) {
                    spawnLocation = new Vector3(player.transform.position.x + randX, player.transform.position.y + 10, 0);
                }

                if (player.transform.position.y <= 128) {
                    GameObject newEntity = Instantiate(clothBoss, spawnLocation, Quaternion.identity);
                }
            }
        }

        if (GameObject.FindGameObjectsWithTag("Boss").Length > 0) {
            gameManager.DisablePortals();
        } else {
            gameManager.EnablePortals();
        }
    }

    public void SetClothBossSpawnDay(int dayNo) {
        clothBossSpawnDay = Math.Min(clothBossSpawnDay, dayNo);
    }
}
