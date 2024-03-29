using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MobType { cow, chicken, pig, skeleton, zombie, slime, slimeking, cursedcloth, clothboss };

public class MobStats : MonoBehaviour {

    public static MobStats instance;
    public int[] kills;
    private int noOfMobs;
    private GameObject entitySpawner;

    public void Awake() {
        if (instance == null) {
            noOfMobs = MobType.GetNames(typeof(MobType)).Length;
            kills = new int[noOfMobs];
            instance = this;
            entitySpawner = GameObject.Find("EntitySpawner");
        } else {
            Destroy(gameObject);
        }
    }

    public void addKill(MobType type) {
        instance.kills[(int) type] += 1;
        if (type == MobType.pig && instance.kills[(int) MobType.pig] == 2) {
            Achievement.instance.UnlockAchievement(Achievement.AchievementType.technoblade);
        }

        if (type == MobType.slimeking) {
            entitySpawner.GetComponent<EnemySpawner>().EnableSlime();
        }

        if (type == MobType.clothboss) {
            entitySpawner.GetComponent<EnemySpawner>().EnableCloth();
        }
    }

}
