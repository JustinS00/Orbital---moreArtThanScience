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

    // 1.0f == 1s
    private float fireRate = 1.0f;
    private float nextFireTime;

    // Update is called once per frame
    void Update() {
        base.LookAtPlayer();
        AttackPlayer();
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
        Instantiate(arrow, arrowParent.transform.position, Quaternion.identity);
        nextFireTime = Time.time + fireRate;
    }
}
