using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

	[Header("Zombie Settings")]
	public int zombieDamage = 10;
	public int maxHealth = 100;
	int currentHealth;

	public Transform player;

	private bool isFlipped = false;

	public PlayerController target;

	private Rigidbody2D rb;
	public int knockback = 10;

	void Start() {
		currentHealth = maxHealth;
		rb = GetComponent<Rigidbody2D>();
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
		currentHealth -= damage;

		//knockback
		if (fromRight)
			rb.AddForce(new Vector2(-2, 2), ForceMode2D.Impulse);
		else 
			rb.AddForce(new Vector2(2, 2), ForceMode2D.Impulse);

		// damage animation

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
			bool fromRight = other.transform.position.x < transform.position.x
				? true
				: false;
			other.gameObject.GetComponent<PlayerController>().TakeDamage(zombieDamage, fromRight);
		}
	}
	

	/* Might be better to change to OnCollisionEnter2D with Invoke Repeating */


	/*
	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {	
			target = other.gameObject.GetComponent<PlayerController>();
			InvokeRepeating("Attack", 0.1f, 2.0f);
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
			target.TakeDamage(zombieDamage);
		}
	}

	*/
	
}