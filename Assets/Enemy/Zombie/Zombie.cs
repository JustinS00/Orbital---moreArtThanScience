using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

	[Header("Zombie Settings")]
	public int zombieDamage = 10;

	public Transform player;

	public bool isFlipped = false;

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

}