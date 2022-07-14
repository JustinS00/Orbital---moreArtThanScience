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

    enum BossType {
        slimeking,
        flyboss
    }

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        entityPrefabs = entityCollection.prefabs;
    }

    void Update() {
        int dayNo = gameManager.getDayNo();
        if (MobStats.instance.kills[(int) MobType.slimeking] == 0) {
            if (dayNo >= 10 && dayNo % 5 == 0 && GameObject.FindGameObjectsWithTag("Boss").Length == 0) {
                Debug.Log(dayNo);
                int slimekingIndex = (int) BossType.slimeking;
                GameObject slimeking = entityPrefabs[slimekingIndex];
                spawnLocation = new Vector3(player.transform.position.x + randX, player.transform.position.y + 10, 0);
                while (Physics2D.OverlapCircle(spawnLocation, 1.0f) != null) {
                    spawnLocation = new Vector3(player.transform.position.x + randX, player.transform.position.y + 10, 0);
                }
                if (player.transform.position.y <= 128) {
                    GameObject newEntity = Instantiate(slimeking, spawnLocation, Quaternion.identity);
                }
            }
        }
    }
}
