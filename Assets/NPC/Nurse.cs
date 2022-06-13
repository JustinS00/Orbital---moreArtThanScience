using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurse : NPC
{   
    public override void startAction(PlayerController player) {
        player.healthBar.SetMaxHealth(player.maxHealth);
        player.health.SetFullHealth();
    }
    public override void stopAction(PlayerController player) {
        return;
    }
}
