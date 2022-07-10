using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Scan : MonoBehaviour {

    private AstarPath path;

    private Transform player;

    // Start is called before the first frame update
    void Start() {
        path = GetComponent<AstarPath>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("updatePath", 0, 5f);
    }

    // Update is called once per frame
    void Update() {
    }

    private void updatePath() {
        var graph = AstarPath.active.data.FindGraph(g => g.name == "Flying");

        var gg = AstarPath.active.data.gridGraph;
        gg.center = new Vector2(player.position.x, player.position.y);
        path.Scan();
    }
}
