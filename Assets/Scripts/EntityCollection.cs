using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityCollection", menuName = "ScriptableObjects/Entity Collection")]
public class EntityCollection : ScriptableObject {

    [SerializeField]
    public GameObject[] prefabs;
}