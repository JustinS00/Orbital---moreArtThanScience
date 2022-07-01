using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Achievement : MonoBehaviour
{   

    private GameObject achievementUI;
    private GameObject achievementPopUp;
    private GameManager gameManager;
    public static Achievement instance;
    private GameObject[] achievements;
    private bool[] achievementsUnlocked = new bool[9];
    public enum AchievementType {willsmith, diamondhands, luna, besttrade, siu, deckedout, emotionaldamage, technoblade};
    private float popUpTimer = 5f;
    private float showTimer = 0f;

    private void Awake() {
        
        achievementUI = GameObject.Find("Achievement");
        achievementPopUp = GameObject.Find("Achievement PopUp");
        achievementPopUp.SetActive(false);
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        achievements = GameObject.FindGameObjectsWithTag("Achievement Template");
        foreach (GameObject achievement in achievements) {
            achievement.transform.Find("Image").gameObject.SetActive(false);
            achievement.transform.Find("Title").gameObject.SetActive(false);
        }
        
        achievementUI.SetActive(false);
        achievementPopUp.SetActive(false);
        instance = this;
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
            case AchievementType.technoblade:
                UpdateAchievement(7);
                break;   
            default:
                break;         
        }       
        PlaySound(achievement);
    }

    private void Update() {
        showTimer -= Time.deltaTime;
        if (showTimer <= 0f) {
            HidePopUp();
        }
    }

    private void UpdateAchievement(int achievementIndex) {
        if (!achievementsUnlocked[achievementIndex]) {
            string achievement_name = "Achievement Template " + achievementIndex.ToString();
            GameObject achievement = achievementUI.transform.Find(achievement_name).gameObject;
            Debug.Log(achievement);
            achievement.transform.Find("Image").gameObject.SetActive(true);
            achievement.transform.Find("Title").gameObject.SetActive(true);
            ShowPopUp(achievement);
     
            //Debug.Log(achievements[achievementIndex].transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text);
            achievementsUnlocked[achievementIndex] = true;
        }
    }

    private void ShowPopUp(GameObject achievement) {
        
        achievementPopUp.transform.Find("Achievement Template/Image").gameObject.GetComponent<Image>().sprite = achievement.transform.Find("Image").gameObject.GetComponent<Image>().sprite;
        achievementPopUp.transform.Find("Achievement Template/Title").gameObject.GetComponent<TextMeshProUGUI>().text = achievement.transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text;
        achievementPopUp.transform.Find("Achievement Template/Details").gameObject.GetComponent<TextMeshProUGUI>().text = achievement.transform.Find("Details").gameObject.GetComponent<TextMeshProUGUI>().text;
        
        achievementPopUp.SetActive(true);
        showTimer = popUpTimer;
    
    }

    private void PlaySound(AchievementType achievement) {
        switch (achievement) {         
            case AchievementType.emotionaldamage:
                AudioManager.instance.PlaySound("emotional_damage");
                break;   
            case AchievementType.technoblade:
                AudioManager.instance.PlaySound("technoblade");
                break;   
            default:
                break;         
        }     
    }


    private void HidePopUp() {
        achievementPopUp.SetActive(false);
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
