using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    protected int damage = 5;
    [SerializeField]
    protected float speed = 1.5f;

    [SerializeField]
    private EnemyData data;

    protected GameObject player;
    private GameObject target;
    protected Rigidbody2D rb;
    protected Animator anim;

    [SerializeField]
    private int maxDistanceFromPlayer = 50;

    protected bool isFlipped = false;

    [SerializeField]
    private bool isRanged = false;

    [Header("Death Effects")]
    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private LootTable lootTable;

    public AudioSource idleSound;
    public AudioSource damagedSound;
    private float soundChance = 0.003f;
    
    // Start is called before the first frame update
    protected void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        SetEnemyValues();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        LookAtPlayer();
        AttackPlayer();
        PlayIdleSound();
    }

    private void SetEnemyValues() {
        GetComponent<Health>().SetHealth(data.health, data.health);
        damage = data.damage;
        speed = data.speed;
    }

    protected void LookAtPlayer() {
        // Despawns enemy if distance > maxDistanceFromPlayer
        if (Vector2.Distance(player.transform.position, transform.position) > maxDistanceFromPlayer) {
            Destroy(gameObject);
        }
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.transform.position.x && isFlipped) {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        } else if (transform.position.x < player.transform.position.x && !isFlipped) {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public virtual void AttackPlayer() {
        // run to player
        if (!isRanged) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            /*
            knockback related
			fromRight = other.transform.position.x < transform.position.x
				? true
				: false;	
            */
            target = other.gameObject;
            InvokeRepeating("Attack", 0.1f, 1.0f);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            target = null;
            CancelInvoke("Attack");
        }
    }

    private void Attack() {
        if (target != null) {
            target.GetComponent<Health>().Damage(damage);
            PlayIdleSound();
        }
    }

    public void MakeLoot() {
        if (lootTable) {
            int randNumItems = Random.Range(0, data.maxItemsToDrop + 1);
            lootTable.generateLoot(randNumItems, transform.position);
        }
    }

    public void PlayDamagedSound() {
        try {
            damagedSound.Play();
        } catch {
            Debug.Log("this does not have damaged audio source attached");
        }
    }

    public void PlayIdleSound() {
        if (Random.value <= soundChance) {
            try {
                idleSound.Play();
            } catch {
                Debug.Log("this does not have idle audio source attached");
            }
        }
    }
}
