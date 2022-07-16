using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : MonoBehaviour {

    private Health health;
    private Animator anim;
    private SlimeAttack attackScript;

    #region SpinSkill
    [SerializeField]
    private GameObject slimeProjectilePrefab;
    [SerializeField]
    private float spinIdleTimeSuccess = 25f;
    [SerializeField]
    private float spinIdleTimeFail = 10f;
    private float _nextSpinTime;
    [SerializeField]
    private float spinChance = 1f;
    private int maxNumberOfProjectiles = 3;

    [SerializeField]
    private Transform spawnPoint;

    private float maxLaunchForce = 5f;
    private float sleepTime = 20f;
    #endregion


    // Start is called before the first frame update
    void Start() {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        attackScript = GetComponent<SlimeAttack>();
    }

    // Update is called once per frame
    void Update() {
        if (health.GetHealthPercentage() <= 0.5) {
            if (Time.time >= _nextSpinTime) {
                float spinProbability = Random.Range(0, 1.0f);
                // 40% chance to spin every 15 seconds
                if (spinProbability <= spinChance) {
                    Spin();
                    _nextSpinTime = Time.time + spinIdleTimeSuccess;
                } else {
                    _nextSpinTime = Time.time + spinIdleTimeFail;
                }
            }
        }
    }

    //spin animation, shoot projectiles and spawn slimes oncollision
    private void Spin() {
        anim.SetTrigger("spin");

        StartCoroutine(SpawnSlimes(maxNumberOfProjectiles));
        StartCoroutine(Sleep());
    }

    private IEnumerator Sleep() {
        attackScript.ToggleSleep();
        yield return new WaitForSeconds(sleepTime);
        attackScript.ToggleSleep();
    }

    private IEnumerator SpawnSlimes(int numberOfProjectiles) {
        int maxNumEntities = OptionsMenu.instance.GetHostileMobCap();
        if (numberOfProjectiles > 0 && GameObject.FindGameObjectsWithTag("Enemy").Length < maxNumEntities) {
            GameObject slime = Instantiate(slimeProjectilePrefab, spawnPoint.position, Quaternion.identity);

            int direction = Random.Range(0, 1.0f) > 0.5 ? 1 : -1;
            Vector2 randVelocity = Vector2.one * direction * Random.Range(1, maxLaunchForce);
            slime.GetComponent<Rigidbody2D>().velocity = randVelocity + (Vector2.up * 5);

            yield return new WaitForSeconds(1.5f);
            StartCoroutine(SpawnSlimes(numberOfProjectiles - 1));
        }
    }
}
