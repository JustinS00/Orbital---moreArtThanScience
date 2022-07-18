using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FlyingAI : Enemy {
    // Start is called before the first frame update

    protected new void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected void Update() {
        base.LookAtPlayer();
    }
}
