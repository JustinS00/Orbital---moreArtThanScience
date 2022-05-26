using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

	[Header("Zombie Settings")]
	public int zombieDamage = 10;

	public Transform player;

	public bool isFlipped = false;


	public PlayerController target;

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

	
	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {	
			other.gameObject.GetComponent<PlayerController>().TakeDamage(zombieDamage);
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