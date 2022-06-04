using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private float spawnInterval = 30.0f;

    [SerializeField]
    private int maxNumEnemies = 5;
    
    private int numberOfPossibleEnemies;
    private Vector3 spawnLocation;
    private float randX;

    private GameObject player;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        numberOfPossibleEnemies = enemyPrefabs.Length;
        StartCoroutine(spawnEnemy(spawnInterval, enemyPrefabs[Random.Range(0, numberOfPossibleEnemies)]));
    }

    // Update is called once per frame
    void Update() {
        
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy) {
       
        yield return new WaitForSeconds(interval);

        randX = Random.Range(-10.5f, 10.5f);
        // to make the range [-15.5, -5.5], [5.5, 15.5] so don't spawn right beside player
        if (randX > 0) {
            randX += 5.0f;
        } else {
            randX -= 5.0f;
        }

        spawnLocation = new Vector3(player.transform.position.x + randX, player.transform.position.y + 10, 0);
        GameObject newEnemy = Instantiate(enemy, spawnLocation, Quaternion.identity);

        //limits number of enemies spawned to maxNumEnemies
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length < maxNumEnemies);
        StartCoroutine(spawnEnemy(spawnInterval, enemyPrefabs[Random.Range(0, numberOfPossibleEnemies)]));
    }
}
