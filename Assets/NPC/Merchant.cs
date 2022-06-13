using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : NPC
{   
    private bool isShowing;
    private TradingUI tradeUI;
    private PlayerController player;

    private void Start() {
        tradeUI = GameObject.Find("Trading UI").GetComponent<TradingUI>();
    }
    public override void startAction(PlayerController player) {
        this.player = player;
    
    }
    public override void stopAction(PlayerController player) {
        this.player = null;
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
