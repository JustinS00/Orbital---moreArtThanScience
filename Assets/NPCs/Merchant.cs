using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : NPC
{   
    private bool isShowing;
    private TradingUI tradeUI;
    private PlayerController player;
    private string message = "Press 'E' to trade";
    private GameManager gameManager;

    private void Awake() {
        tradeUI = GameObject.Find("Trading UI").GetComponent<TradingUI>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public override void startAction(PlayerController player) {
        PopUp.ShowPopUp_Static(message);
        this.player = player;
        if (player.showInv) {
            ToggleTradeUI();
        }
    
    }
    public override void stopAction(PlayerController player) {
        if (isShowing) {
            ToggleTradeUI();
        }
        this.player = null;
        PopUp.HidePopUp_Static();
    }

    public void Update() {
        if (gameManager.isGamePaused()) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            ToggleTradeUI();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isShowing) {
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
