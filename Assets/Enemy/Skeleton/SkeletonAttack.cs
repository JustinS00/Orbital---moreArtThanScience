using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : Enemy {

    [SerializeField]
    private int attackRange = 12;

    [SerializeField]
    private GameObject arrow;

    [SerializeField]
    private GameObject arrowParent;

    private float launchForce = 15.0f;

    // 1.0f == 1s
    private float fireRate = 4.0f;
    private float nextFireTime;

    // Update is called once per frame
    void Update() {
        base.LookAtPlayer();
        AttackPlayer();
        PlayIdleSound();
        onGround = -0.05f <= rb.velocity.y && rb.velocity.y <= 0.05f;
        if (onGround) {
            StartCoroutine(TryJump());
        } else {
            StopCoroutine(TryJump());
        }
    }

    public override void AttackPlayer() {
        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);

        if (distanceFromPlayer > attackRange) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        } else if (distanceFromPlayer <= attackRange && nextFireTime < Time.time) {
            Shoot();
        }
    }

    private void Shoot() {
        if (Physics2D.OverlapCircle(arrowParent.transform.position, 0.5f) != null) return;

        GameObject arrowShot = Instantiate(arrow, arrowParent.transform.position, arrowParent.transform.rotation);

        int direction = base.isFlipped ? 1 : -1;
        int randYPower = Random.Range(1, 5);
        arrowShot.GetComponent<Rigidbody2D>().velocity = Vector2.right * direction * launchForce + Vector2.up * randYPower;

        nextFireTime = Time.time + fireRate;
    }
}
