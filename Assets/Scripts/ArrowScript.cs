using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    private Rigidbody2D rb;
    
    private float speed = 10.0f;
    private int damage;

    private bool hasHit;
    private float destroyTimer = 5.0f;
    
    // Start is called before the first frame update
    void Start() {
        //damage = GetComponent<Enemy>().damage;
        rb = GetComponent<Rigidbody2D>();

        // destroy after 10s
        Destroy(gameObject, destroyTimer);
    }

    void Update() {
        if (!hasHit) {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject);
        hasHit = true;
        rb.isKinematic = true;

        //disables collision of arrow 
        GameObject arrowColliderObject = gameObject.transform.GetChild(0).gameObject;
        arrowColliderObject.GetComponent<CapsuleCollider2D>().enabled = false;

        //sets arrow parent to collided object to make it stick
        transform.parent = other.transform;

        if (other.gameObject.tag == "Player") {
            // TODO get damage
            other.gameObject.GetComponent<Health>().Damage(5);
        }
    }

    
}
