using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : NPC
{   
    private bool isShowing;
    private TradingUI tradeUI;
    private PlayerController player;
    private string message = "Press 'E' to trade";

    private void Awake() {
        tradeUI = GameObject.Find("Trading UI").GetComponent<TradingUI>();
    }
    public override void startAction(PlayerController player) {
        PopUp.ShowPopUp_Static(message);
        this.player = player;
    
    }
    public override void stopAction(PlayerController player) {
        this.player = null;
        PopUp.HidePopUp_Static();
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            
            ToggleTradeUI();
        }
    }

    public void ToggleTradeUI() {
        isShowing = !isShowing;
        if (isShowing && player != null) {
            tradeUI.Show(player);
        } else if(!isShowing && player != null) {
            tradeUI.Hide(player);
        }       
    }
}
