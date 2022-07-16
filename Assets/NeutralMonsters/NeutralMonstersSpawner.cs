using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralMonstersSpawner : MonoBehaviour {

    [SerializeField]
    private EntityCollection entityCollection;
    private GameObject[] entityPrefabs;

    [SerializeField]
    private float spawnInterval = 25.0f;

    [SerializeField]
    private int maxNumEntities;

    private int numberOfPossibleEntities; 
    private Vector3 spawnLocation;
    private float randX;
    private string currentTag;

    private GameObject player;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        entityPrefabs = entityCollection.prefabs;

        numberOfPossibleEntities = entityPrefabs.Length;
        currentTag = entityPrefabs[0].tag;

        StartCoroutine(spawnEntity(spawnInterval, entityPrefabs[Random.Range(0, numberOfPossibleEntities)]));
    }

    private IEnumerator spawnEntity(float interval, GameObject entity) {
        yield return new WaitForSeconds(interval);

        randX = Random.Range(-25.5f, 25.5f);
        // make spawn out of camera
        if (randX > 0) {
            randX += 5.0f;
        } else {
            randX -= 5.0f;
        }

        spawnLocation = new Vector3(player.transform.position.x + randX, player.transform.position.y + 10, 0);
        if (Physics2D.OverlapCircle(spawnLocation, 1.0f) == null && player.transform.position.y <= 128) {
            GameObject newEntity = Instantiate(entity, spawnLocation, Quaternion.identity);
        }

        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag(currentTag).Length < maxNumEntities);
        StartCoroutine(spawnEntity(spawnInterval, entityPrefabs[Random.Range(0, numberOfPossibleEntities)]));
    }
}
