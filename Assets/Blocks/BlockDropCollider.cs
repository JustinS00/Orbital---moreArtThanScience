using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDropCollider : MonoBehaviour
{   
    public bool touchingPlayer;
    public ItemClass item;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            touchingPlayer = true;
            //Add to player inv
            if (col.GetComponent<Inventory>().Add(item)) {
                Destroy(this.gameObject);
            } 
        }
    }
}
