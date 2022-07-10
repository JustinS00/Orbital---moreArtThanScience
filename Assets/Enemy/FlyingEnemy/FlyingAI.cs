using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAI : Enemy {
    // Start is called before the first frame update
    private new void Start() {
        base.Start();
        AttackPlayer();
    }

    // Update is called once per frame
    void Update() {
        base.LookAtPlayer();
    }

    public override void AttackPlayer() {
        
    }
}
