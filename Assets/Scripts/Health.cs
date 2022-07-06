using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField]
    private int health = 40;

    [SerializeField]
    private float invincibilityDurationSeconds = 1.0f;
    public bool isInvincible;

    private Animator anim;

    private int MAX_HEALTH = 40;

    public int protectionValue;

    private float HEAL_COOLDOWN = 1.0f;

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

    public void SetFullHealth() {
        this.health = this.MAX_HEALTH;
    }

    public int GetHealth() {
        return this.health;
    }

    public int GetMaxHealth() {
        return this.MAX_HEALTH;
    }

    public float GetHealthPercentage() {
        return this.health / this.MAX_HEALTH;
    }

    public bool CanDamage(int damage) {
        if (isInvincible || damage < 0)
            return false;
        return true;
    }

    public void Damage(int damage) {
        if (isInvincible) return;

        if (damage < 0) {
            throw new System.ArgumentOutOfRangeException("No negative damage");
        }
        
     
        float newDamage = Mathf.RoundToInt(damage * (100 - 2 * protectionValue) / 100.0f);
        
        // Armour durability related
        if (gameObject.tag == "Player") {
            int durabilityLost = Mathf.Max(1, damage / 10);
            GetComponent<PlayerController>().armourDamage(durabilityLost);
            newDamage *= OptionsMenu.instance.GetMultiplier();
        }

        this.health -= Mathf.RoundToInt(newDamage); 
        Debug.Log(this.name + "got hit");
        // hit animation
        anim.SetTrigger("damaged");

        // add IFrame
        if (!isInvincible) {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    public void Heal(int health) {
        if (health < 0) {
            throw new System.ArgumentOutOfRangeException("No negative damage");
        }

        StartCoroutine(AddHealth(health));
    }


    public IEnumerator AddHealth(int health) {
        for (int i = 0; i < health; i++)
        {
            this.health = Mathf.Min(MAX_HEALTH, this.health + 1);
            yield return new WaitForSeconds(HEAL_COOLDOWN);
        }
    }

    private void Die() {
        if (GetComponent<Enemy>()) {
            GetComponent<Enemy>().MakeLoot();
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
        // probably can make this better by seperating into a makeloot script instead of duplicating
        if (GetComponent<NeutralMonster>()) {
            GetComponent<NeutralMonster>().MakeLoot();
            //Destroy(gameObject);
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
