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
        if (Input.GetKeyDown(KeyCode.H)) {
            Damage(10);
        } 
        if (Input.GetKeyDown(KeyCode.L)) {
            Heal(10);
        } 
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
        Debug.Log(newDamage);
        this.health -= newDamage;
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
        Debug.Log("Die");
    }

    private IEnumerator BecomeTemporarilyInvincible() {
        Debug.Log("Enemy turned invincible!");
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityDurationSeconds);

        isInvincible = false;
        Debug.Log("Enemy is no longer invincible!");
    }
}
