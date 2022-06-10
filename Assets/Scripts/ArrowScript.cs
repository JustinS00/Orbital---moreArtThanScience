using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    private GameObject target;
    
    private Rigidbody2D rb;
    
    private float speed = 10.0f;
    private int damage;
    
    // Start is called before the first frame update
    void Start() {
    
        //damage = GetComponent<Enemy>().damage;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");

		if (transform.position.x < target.transform.position.x) {
			transform.Rotate(0f, 0f, -50f);
		}
		else if (transform.position.x > target.transform.position.x) {
			transform.Rotate(0f, 180f, -50f);
		}

        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(moveDir.x, moveDir.y);

        Destroy(this.gameObject, 2);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            // TODO get damage
            other.gameObject.GetComponent<Health>().Damage(5);
        } else if (other.gameObject.tag != "Enemy") {
            Destroy(gameObject);
        }
        
    }

    
}
