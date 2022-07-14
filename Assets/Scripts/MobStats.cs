using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MobType { cow, chicken, pig, skeleton, zombie, slime, slimeking };

public class MobStats : MonoBehaviour {

    public static MobStats instance;
    public int[] kills;
    private int noOfMobs = 6;

    public void Awake() {
        if (instance == null) {
            kills = new int[noOfMobs];
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }


    public void addKill(MobType type) {
        instance.kills[(int) type] += 2;
        if (instance.kills[(int) MobType.pig] == 2) {
            Achievement.instance.UnlockAchievement(Achievement.AchievementType.technoblade);
        }
        /*
        switch (type) {
            case MobType.cow:
                instance.kills[0] += 1;
                break;
            case MobType.chicken:
                instance.kills[1] += 1;
                break;  
            case MobType.pig:
                instance.kills[2] += 1;
                if (instance.kills[2] == 2) {
                    Achievement.instance.UnlockAchievement(Achievement.AchievementType.technoblade);
                }
                break;  
            case MobType.skeleton:
                instance.kills[3] += 1;
                break;   
            case MobType.zombie:
                instance.kills[4] += 1;
                break;    
             case MobType.slime:
                instance.kills[5] += 1;
                break;
            case MobType.slimeking:
                instance.kills[6] += 1;
                break;
            default:
                break;         
        }   
        */
    }

}
