using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            startAction(col.gameObject.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            stopAction(col.gameObject.GetComponent<PlayerController>()) ;
        }      
    }

    public abstract void startAction(PlayerController player);
    public abstract void stopAction(PlayerController player);
}
