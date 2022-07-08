using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {   

    private string message;
    private string enableMessage;
    private string disableMessage;

    private Vector2 toLocation;
    private PlayerController player;
    private bool inRange;
    private bool portalIsOn;


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

    public void SetEnableMessage(string message) {
        this.enableMessage = message;
    }

    public void SetDisableMessage(string message) {
        this.disableMessage = message;
    }

    public void SetLocation(Vector2 location) {
        this.toLocation = location;
    }

    public void TogglePortalOn() {
        this.message = enableMessage;
        portalIsOn = true;
    }

    public void TogglePortalOff() {
        this.message = disableMessage;
        portalIsOn = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T) && inRange && portalIsOn) {
            player.moveTo(toLocation);
        }
    }
}
