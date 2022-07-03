using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour {

    private Rigidbody2D rb;

    [SerializeField] private GameObject slimePrefab;
    private bool spawnedSlime = false;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!spawnedSlime) {
            Instantiate(slimePrefab, transform.position, Quaternion.identity);
            spawnedSlime = true;
        }
        gameObject.SetActive(false);
    }
}
