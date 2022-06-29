using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableArrowAtStart : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(disableAtStart());
    }

    private IEnumerator disableAtStart() {
        yield return new WaitForSeconds(0.01f);
        GetComponent<Collider2D>().enabled = true;
    }
}
