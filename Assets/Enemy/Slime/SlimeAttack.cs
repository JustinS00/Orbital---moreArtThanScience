using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : Enemy {


    [SerializeField]
    private float maxYAxisSpeedMultiplier = 2.5f;

    private float lastYPos;

    private bool isGrounded = false;
    private bool isSleeping = false;
    private bool isFalling = false;

    // related to attack cooldown
    private float idleTime = 3f;
    private float nextAttackTime;
    private bool isIdle = true;

    public enum Animations {
        Idle = 0,
        Jumping = 1,
        Falling = 2
    };

    private Animations currentAnimation;

    // Update is called once per frame
    void Update() {
        if (!isSleeping) {
            base.LookAtPlayer();
        }
        PlayIdleSound();

    }

    void FixedUpdate() {
        if (isIdle) {
            if (Time.time >= nextAttackTime && !isSleeping) {
                AttackPlayer();
                nextAttackTime = Time.time + idleTime;
            }
        }

        isGrounded = -0.1f <= base.rb.velocity.y && base.rb.velocity.y <= 0.1f;
        // just hit ground
        if (isGrounded && isFalling) {
            isFalling = false;
            isIdle = true;
            ChangeAnimation(Animations.Idle);
        } else if (transform.position.y > lastYPos && !isGrounded && !isIdle) {
            // going up and not grounded
            isFalling = false;
            ChangeAnimation(Animations.Jumping);
        } else if (transform.position.y < lastYPos && !isGrounded && !isIdle) {
            // going down and not grounded
            isFalling = true;
            ChangeAnimation(Animations.Falling);
        }

        lastYPos = transform.position.y;
    }

    public override void AttackPlayer() {
        isIdle = false;
        int direction = base.isFlipped ? 1 : -1;
        float randYMultiplier = Random.Range(1f, maxYAxisSpeedMultiplier);
        rb.velocity = new Vector2(speed * direction, speed * randYMultiplier);
    }

    public void ToggleSleep() {
        this.isSleeping = !this.isSleeping;
    }

    private void ChangeAnimation(Animations newAnimation) {
        if (currentAnimation != newAnimation) {
            currentAnimation = newAnimation;
            anim.SetInteger("state", (int) newAnimation);
        }
    }
}
