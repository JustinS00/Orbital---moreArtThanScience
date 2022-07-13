using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralMonster : MonoBehaviour {

    private Rigidbody2D rb;
    private GameObject player;

    private Animator anim;
    private bool animIsInitialized = false;

    [SerializeField]
    protected float speed = 1.5f;
    private bool onGround;

    [SerializeField]
    private EnemyData data;
    
    [SerializeField]
    private LootTable lootTable;

    [SerializeField]
    private float maxDistanceFromPlayer = 100.0f;
    private bool isFlipped;

    private int minIdleTime = 4;
    private int maxIdleTime = 8;
    private int minMoveTime = 2;
    private int maxMoveTime = 5;

    public AudioSource idleSound;
    public AudioSource damagedSound;
    public MobType type;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        SetEnemyValues();

        anim = GetComponent<Animator>();
        StartCoroutine(InitializeAnimator());
        StartCoroutine(MovementBehavior());
    }

    // Update is called once per frame
    void Update() {
        // Despawns enemy if distance > maxDistanceFromPlayer
        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceFromPlayer > maxDistanceFromPlayer) {
            Destroy(gameObject);
        }

        if (animIsInitialized) {
            if (-0.1f <= rb.velocity.x && rb.velocity.x <= 0.1f) {
                anim.SetBool("toWalk", false);
            } else {
                anim.SetBool("toWalk", true);
            }
        }
    }

    private void SetEnemyValues() {
        GetComponent<Health>().SetHealth(data.health, data.health);
        speed = data.speed;
    }

    public void MakeLoot() {
        if (lootTable) {
            lootTable.generateLoot(data.maxItemsToDrop + 1, transform.position);
        }
    }

    private IEnumerator InitializeAnimator() {
        yield return new WaitUntil(() => anim.isInitialized);
        animIsInitialized = true;
    }

    private IEnumerator MovementBehavior() {
        onGround = -0.1f <= rb.velocity.y && rb.velocity.y <= 0.1f;
        if (onGround) {
            float rand = Random.Range(0, 5); //[0, 5)
            if (rand <= 2) {
                rb.velocity = Vector2.zero;
                int timeToIdle = Random.Range(minIdleTime, maxIdleTime);
                if (timeToIdle == minIdleTime) {
                    try {
                        idleSound.Play();
                    } catch {
                        Debug.Log("this does not have idle audio source attached");
                    }
                }
                yield return new WaitForSeconds(timeToIdle);
            } else {
                // direction == 1 right, else left
                int direction = rand % 2 == 0 ? 1 : -1;
                Flip(direction);
                
                rb.velocity = new Vector2(direction * speed, rb.velocity.y);
                //anim.SetBool("toWalk", true);

                int timeToMove = Random.Range(minMoveTime, maxMoveTime);
                yield return new WaitForSeconds(timeToMove);

                //anim.SetBool("toWalk", false);
            }
            StartCoroutine(MovementBehavior());
        }
    }

    private void Flip(int direction) {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (direction < 0 && isFlipped) {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        } else if (direction > 0 && !isFlipped) {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Neutral Monster") {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    public void PlayDamagedSound() {
        try {
            damagedSound.Play();
        } catch {
            Debug.Log("this does not have damaged audio source attached");
        }
    }
}
