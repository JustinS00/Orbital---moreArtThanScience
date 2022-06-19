using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField]
    private int health = 100;

    [SerializeField]
    private float invincibilityDurationSeconds = 1.0f;
    public bool isInvincible;

    private Animator anim;

    private int MAX_HEALTH = 100;

    public int protectionValue;

    void Start() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (health <= 0) Die();
    }

    public void SetHealth(int maxHealth, int health) {
        this.MAX_HEALTH = maxHealth;
        this.health = health;
    }

    public int getHealth() {
        return this.health;
    }

    public void SetFullHealth() {
        this.health = this.MAX_HEALTH;
    }

    public void Damage(int damage) {
        if (isInvincible) return;

        if (damage < 0) {
            throw new System.ArgumentOutOfRangeException("No negative damage");
        }

        int newDamage = Mathf.RoundToInt(damage * (100 - 2 * protectionValue) / 100.0f);
        this.health -= newDamage;

        // Armour durability related
        if (gameObject.tag == "Player") {
            int durabilityLost = Mathf.Max(1, damage / 10);
            GetComponent<PlayerController>().armourDamage(durabilityLost);
        }

        Debug.Log(this.name + "got hit");
        // hit animation
        anim.SetTrigger("damaged");

        // add IFrame
        if (!isInvincible) {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    public void Heal(int heal) {
        if (heal < 0) {
            throw new System.ArgumentOutOfRangeException("No negative damage");
        }

        if (health + heal > MAX_HEALTH) {
            this.health = MAX_HEALTH;
        } else {
            this.health += heal;
        }
    }

    private void Die() {
        if (GetComponent<Enemy>()) {
            GetComponent<Enemy>().MakeLoot();
            gameObject.SetActive(false);
        }
        Debug.Log("Die");
    }

    private IEnumerator BecomeTemporarilyInvincible() {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDurationSeconds);

        isInvincible = false;
    }
}
