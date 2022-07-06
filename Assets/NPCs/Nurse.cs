using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurse : NPC
{   
    private string message = "Go closer to the Nurse to heal";

    public override void startAction(PlayerController player) {
        player.health.Heal(player.health.GetMaxHealth() - player.health.GetHealth());
        PopUp.ShowPopUp_Static(message);
    }
    public override void stopAction(PlayerController player) {
        PopUp.HidePopUp_Static();
    }
}
