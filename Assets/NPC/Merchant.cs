using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : NPC
{   

    private TradingUI tradeUI;

    private void Start() {
        tradeUI = GameObject.Find("Trading UI").GetComponent<TradingUI>();
    }
    public override void startAction(PlayerController player) {
        tradeUI.Show(player);
    }
    public override void stopAction(PlayerController player) {
        tradeUI.Hide(player);
    }
}
