using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingBoss : FlyingAI {
    // Start is called before the first frame update

    #region Components
    private AIDestinationSetter destinationSetter;
    private Health health;
    private Animator animator;
    private Collider2D playerCollider2D;
    private Collider2D thisCollider2D;
    private AIPath pathfinder;
    #endregion

    #region Pre-Rage
    [SerializeField] private GameObject cursedCloth;
    [SerializeField] private int maxAddsToSpawn = 4;
    [SerializeField] private int spawnCooldown = 15;
    private float nextTimeToSpawn;
    #endregion

    #region Post-Rage
    private bool _rage = false;

    [SerializeField] private float dashCooldown = 10f;
    [SerializeField] private float dashDuration = 2f;
    private bool startDashing = false;
    [SerializeField] private int dashDamage = 8;
    private bool isDashing = false;
    private int dashPower = 2;
    #endregion

    private new void Start() {
        base.Start();
        destinationSetter = GetComponent<AIDestinationSetter>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        nextTimeToSpawn = Time.time + spawnCooldown;
        playerCollider2D = player.GetComponent<Collider2D>();
        thisCollider2D = GetComponent<Collider2D>();
        pathfinder = GetComponent<AIPath>();
    }

    // Update is called once per frame
    new void Update() {
        base.Update();
        if (!_rage) {
            if (health.GetHealthPercentage() < 0.5) {
                Rage();
            } else {
                int maxNumEntities = OptionsMenu.instance.GetHostileMobCap();
                if (Time.time > nextTimeToSpawn && GameObject.FindGameObjectsWithTag("Enemy").Length < maxNumEntities) {
                    GameObject cloth = Instantiate(cursedCloth, transform.position, Quaternion.identity);
                    cloth.SetActive(true);
                    nextTimeToSpawn = Time.time + spawnCooldown;
                }
            }
        } else {
            if (!startDashing) {
                startDashing = true;
                StartCoroutine(DashCoroutine());
            }
        }
    }

    private void Rage() {
        _rage = true;
        destinationSetter.Rage();
        animator.SetBool("rage", true);
    }

    private IEnumerator DashCoroutine() {
        yield return new WaitForSeconds(dashCooldown);

        isDashing = true;
        pathfinder.canMove = false;
        int directionX = player.transform.position.x > transform.position.x ? 1 : -1;
        destinationSetter.Dash(directionX);
        rb.velocity = new Vector2(directionX * transform.localScale.x * dashPower, transform.localScale.y);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        pathfinder.canMove = true;
        Physics2D.IgnoreCollision(playerCollider2D, thisCollider2D, false);

        StartCoroutine(DashCoroutine());
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (isDashing && collision.gameObject.CompareTag("Player")) {
            Physics2D.IgnoreCollision(playerCollider2D, thisCollider2D, true);
            player.GetComponent<Health>().Damage(dashDamage, this.gameObject);
        }
    }
}