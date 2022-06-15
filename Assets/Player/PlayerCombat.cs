using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    public Animator anim;
    
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 1;

    void Start() {
        enemyLayers = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update() {
    }

    public void Attack(WeaponClass weapon) {
        // Play attack animation
        anim.SetTrigger("attack");
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage them
        foreach(Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Health>().Damage(Mathf.RoundToInt(weapon.damage));
            weapon.reduceDurability(1);
        }
    }

    public void Attack() {
        // Play attack animation
        anim.SetTrigger("attack");
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage them
        foreach(Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Health>().Damage(attackDamage);
            Debug.Log("Will Smith Slap");
        }
    }
    /*
    Can enable this function to see attack circle in scene
    private void OnDrawGizmosSelected() {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    */
}
