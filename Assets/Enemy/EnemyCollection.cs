using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCollection", menuName = "Enemy Collection")]
public class EnemyCollection : ScriptableObject {   

    #region Normal Enemies
    [SerializeField]
    public GameObject[] enemyPrefabs;
    #endregion

    #region Boss

    #endregion
}
