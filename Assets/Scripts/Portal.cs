using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {   

    private string message;
    private Vector2 toLocation;
    private PlayerController player;
    private bool inRange;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            this.player = col.gameObject.GetComponent<PlayerController>();
            inRange = true;
            PopUp.ShowPopUp_Static(message);
        }

    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            this.player = null;
            inRange = false;
            PopUp.HidePopUp_Static();
        }      
    }

    public void SetMessage(string message) {
        this.message = message;
    }

    public void SetLocation(Vector2 location) {
        this.toLocation = location;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T) && inRange) {
            player.moveTo(toLocation);
        }
    }
}
