using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Statistics")]
    [SerializeField] protected int damage = 5;
    [SerializeField] protected float speed = 1.5f;
    [SerializeField] private EnemyData data;

    [Header("Objects")]
    protected GameObject player;
    private GameObject target;
    protected Rigidbody2D rb;
    protected Animator anim;

    [Header("Death Effects")]
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private LootTable lootTable;

    [Header("Audio")]
    public AudioSource idleSound;
    public AudioSource damagedSound;
    private float soundChance = 0.003f;

    [Header("Knockback")]
    [SerializeField] private float knockbackStrength = 5f;
    [SerializeField] private float knockbackTime = 0.6f;
    [SerializeField] private float freezeTime = 1f;
    private bool knockbacked;

    [Header("Misc")]
    protected bool isFlipped = false;
    public MobType type;
    [SerializeField] private int maxDistanceFromPlayer = 50;
    [SerializeField] private bool isRanged = false;

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
        if (!knockbacked && !isRanged) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
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
            target.GetComponent<Health>().Damage(damage, this.gameObject);
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

    public void Knockback(Transform t) {
        Debug.Log("knockback enemy sciript");
        Vector3 dir = transform.position - t.position;
        knockbacked = true;
        rb.velocity = (dir.normalized * knockbackStrength);
        StartCoroutine(UnKnockback());
    }

    private IEnumerator UnKnockback() {
        yield return new WaitForSeconds(knockbackTime);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(freezeTime);
        knockbacked = false;
    }
}
