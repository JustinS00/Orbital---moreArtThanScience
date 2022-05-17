using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDropCollider : MonoBehaviour
{   
    public bool touchingPlayer;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            touchingPlayer = true;
            Destroy(this.gameObject);
            //Add to player inv
        }
    }

}
