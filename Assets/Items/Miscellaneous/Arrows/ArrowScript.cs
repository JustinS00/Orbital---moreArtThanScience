using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    private Rigidbody2D rb;

    [SerializeField]
    private ArrowClass arrowData;

    private bool hasHit;
    private float destroyTimer = 5.0f;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, destroyTimer);
    }

    void Update() {
        if (!hasHit) {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        //disables collision of arrow 
        GameObject arrowColliderObject = gameObject.transform.GetChild(0).gameObject;
        arrowColliderObject.GetComponent<CapsuleCollider2D>().enabled = false;

        //sets arrow parent to collided object to make it stick
        transform.parent = other.transform;

        string colliderTag = other.gameObject.tag;
        if (colliderTag == "Player" || colliderTag == "Enemy" || colliderTag == "Neutral Monster" || colliderTag == "Boss") {
            other.gameObject.GetComponent<Health>().Damage(arrowData.damage, this.gameObject);
        }
    }


}
