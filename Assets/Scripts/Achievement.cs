using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Achievement : MonoBehaviour
{   

    private GameObject achievementUI;
    private GameManager gameManager;
    public static Achievement instance;
    private GameObject[] achievements;
    public enum AchievementType {willsmith, diamondhands, luna, besttrade, siu, deckedout, emotionaldamage};

    private void Awake() {
        
        achievementUI = GameObject.Find("Achievement");
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        achievements = GameObject.FindGameObjectsWithTag("Achievement Template");
        foreach (GameObject achievement in achievements) {
            achievement.transform.Find("Image").gameObject.SetActive(false);
            achievement.transform.Find("Title").gameObject.SetActive(false);
        }
        achievementUI.SetActive(false);
        instance = this;


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UnlockAchievement(AchievementType achievement) {
        switch (achievement) {
            case AchievementType.willsmith:
                UpdateAchievement(0);
                break;
            case AchievementType.diamondhands:
                UpdateAchievement(1);
                break;  
            case AchievementType.luna:
                UpdateAchievement(2);
                break;  
            case AchievementType.besttrade:
                UpdateAchievement(3);
                break;   
            case AchievementType.siu:
                UpdateAchievement(4);
                break;    
             case AchievementType.deckedout:
                UpdateAchievement(5);
                break;   
            case AchievementType.emotionaldamage:
                UpdateAchievement(6);
                break;   

            default:
                break;         
        }       
    }

    private void UpdateAchievement(int achievementIndex) {
        achievements[achievementIndex].transform.Find("Image").gameObject.SetActive(true);
        achievements[achievementIndex].transform.Find("Title").gameObject.SetActive(true);
        Debug.Log(achievements[achievementIndex].transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text);
    }


    public void Back() {
        HideAchievement() ;
    }

    private void ShowAchievement() {
        achievementUI.SetActive(true);
    }
    
    private void HideAchievement() {
        achievementUI.SetActive(false);
    }

    public static void ShowAchievement_Static() {
        instance.ShowAchievement();
    }

    public static void HideAchievement_Static() {
        instance.HideAchievement(); 
    }
}
