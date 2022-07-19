using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    public Animator anim;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public LayerMask bossLayers;

    public float attackRange = 1f;
    public int attackDamage = 1;

    #region Bow/Arrow Related
    [SerializeField]
    private ArrowCollection arrowCollection;
    private float arrowLaunchForce = 2.0f;
    private float nextArrowLaunchTime;

    #endregion


    void Start() {
        enemyLayers = LayerMask.GetMask("Enemy");
        bossLayers = LayerMask.GetMask("Boss");
    }

    // Update is called once per frame
    void Update() {
    }

    public void Attack(WeaponClass weapon) {
        // Play attack animation
        // Debug.Log(OptionsMenu.instance.GetMultiplier());
        anim.SetTrigger("attack");
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        Collider2D[] hitBosses = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, bossLayers);

        var allHitCollider2Ds = hitEnemies.Concat(hitBosses);
        // Damage them
        foreach (Collider2D enemy in allHitCollider2Ds) {
            if (enemy.GetComponent<Health>().CanDamage(Mathf.RoundToInt(weapon.damage))) {
                enemy.GetComponent<Health>().Damage(Mathf.RoundToInt(weapon.damage), this.gameObject);
                weapon.reduceDurability(1);
            }
        }
    }

    public void Attack() {
        // Play attack animation
        anim.SetTrigger("attack");
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        Collider2D[] hitBosses = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, bossLayers);

        var allHitCollider2Ds = hitEnemies.Concat(hitBosses);
        // Damage them
        foreach (Collider2D enemy in allHitCollider2Ds) {
            enemy.GetComponent<Health>().Damage(attackDamage, this.gameObject);
            Achievement.instance.UnlockAchievement(Achievement.AchievementType.willsmith);
        }
    }

    public void Shoot(BowClass bow, ArrowClass arrow) {
        bow.reduceDurability(1);
        nextArrowLaunchTime = Time.time + bow.fireRate / 60;

        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - bowPosition;

        int arrowTypeIndex = (int) arrow.arrowType;
        GameObject arrowToFire = arrowCollection.arrowPrefabs[arrowTypeIndex];

        int dir = direction.x > 0 ? 1 : -1;
        Vector3 arrowPosition = transform.position + dir * Vector3.right;
        GameObject arrowShot = Instantiate(arrowToFire, arrowPosition, attackPoint.rotation);
        arrowShot.GetComponent<Rigidbody2D>().velocity = direction * arrowLaunchForce;
    }

    public bool canFire() {
        return Time.time > nextArrowLaunchTime && Physics2D.OverlapCircle(attackPoint.position, 0.5f) == null;
    }
    /*
    Can enable this function to see attack circle in scene
    private void OnDrawGizmosSelected() {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    */
}
