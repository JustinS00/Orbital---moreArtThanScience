using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{   

    private GameObject achievementUI;
    private GameManager gameManager;
    private static Achievement instance;

    private void Awake() {
        instance = this;
        achievementUI = GameObject.Find("Achievement");
        achievementUI.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
