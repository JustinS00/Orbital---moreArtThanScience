using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Arrows {
    wooden
}

[CreateAssetMenu(fileName = "ArrowCollection", menuName = "Arrow Collection")]
public class ArrowCollection : ScriptableObject
{   

    public GameObject[] arrowPrefabs;

    
}
