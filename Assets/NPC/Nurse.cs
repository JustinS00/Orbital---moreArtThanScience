using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurse: NPC
{   

    public override void startAction(PlayerController player) {
        Heal(player);
    }
    public override void stopAction(PlayerController player) {
    }

    private void Heal(PlayerController player) {
    }
}
