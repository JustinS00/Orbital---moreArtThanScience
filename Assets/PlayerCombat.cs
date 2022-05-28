using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    public Animator anim;
    
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 1;

    // Update is called once per frame
    void Update() {
    }

    public void Attack() {
        // Play attack animation
        anim.SetTrigger("attack");

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage them
        foreach(Collider2D enemy in hitEnemies) {
            bool fromRight = enemy.GetComponent<Transform>().position.x < transform.position.x
				? true
				: false;
            enemy.GetComponent<Zombie>().TakeDamage(attackDamage, fromRight); 
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
