using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float speed = 1.5f;

    [SerializeField]
    private EnemyData data;

    private GameObject player;
    private GameObject target;

    private bool isFlipped = false;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        SetEnemyValues();
    }

    // Update is called once per frame
    void Update() {
        RunAtPlayer(); 
    }

    private void SetEnemyValues() {
        GetComponent<Health>().SetHealth(data.health, data.health);
        damage = data.damage;
        speed = data.speed;
    }

    private void RunAtPlayer() {
        // run to player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        
        // looking at player
        Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.transform.position.x && isFlipped) {
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.transform.position.x && !isFlipped) {
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
    }

    private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
            /*
            knockback related
			fromRight = other.transform.position.x < transform.position.x
				? true
				: false;	
            */
			target = other.gameObject;
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
			target.GetComponent<Health>().Damage(damage);
		}
	}
}
