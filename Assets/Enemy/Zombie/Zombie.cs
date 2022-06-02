using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {
/*
DEPRECATED TO REMOVE
DEPRECATED TO REMOVE
DEPRECATED TO REMOVE
DEPRECATED TO REMOVE
DEPRECATED TO REMOVE
DEPRECATED TO REMOVE
DEPRECATED TO REMOVE
DEPRECATED TO REMOVE

	[Header("Zombie Settings")]
	public int zombieDamage = 10;
	public int maxHealth = 100;
	public int currentHealth;

	public Transform player;
	private Animator anim;

	private bool isFlipped = false;

	public PlayerController target;

	private Rigidbody2D rb;
	public int knockback = 10;
	private bool fromRight;

	[SerializeField]
  	private float invincibilityDurationSeconds = 1.0f;
  	public bool isInvincible;

	void Start() {
		currentHealth = maxHealth;
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

  //Just to help zombie turn
	public void LookAtPlayer() {
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped) {
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped) {
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}


	public void TakeDamage(int damage, bool fromRight) {

		if (isInvincible) return;

		currentHealth -= damage;

		//knockback
		if (fromRight)
			rb.AddForce(new Vector2(-3, 3), ForceMode2D.Impulse);
		else 
			rb.AddForce(new Vector2(3, 3), ForceMode2D.Impulse);

		// damage animation
		anim.SetTrigger("damaged");

		// IFrame
		if (!isInvincible) {
            StartCoroutine(BecomeTemporarilyInvincible());
        }

		if (currentHealth <= 0) {
			Die();
		}
	}

	void Die() {
		// die animation

		// disable enemy
		gameObject.SetActive(false);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			fromRight = other.transform.position.x < transform.position.x
				? true
				: false;	
			target = other.gameObject.GetComponent<PlayerController>();
			InvokeRepeating("Attack", 0.1f, 1.0f);
		} 
	}

	private void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {	
			target = null;
			CancelInvoke("Attack");
		}
	}

	private void Attack() {
		if (target != null) {
			target.TakeDamage(zombieDamage, fromRight);
		}
	}
	
	private IEnumerator BecomeTemporarilyInvincible() {
        Debug.Log("Enemy turned invincible!");
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityDurationSeconds);

        isInvincible = false;
        Debug.Log("Enemy is no longer invincible!");
    }
	*/
}