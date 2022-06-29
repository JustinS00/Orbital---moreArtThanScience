using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static SettingsMenu instance;
    public GameDifficulty difficulty;
    public enum GameDifficulty {easy, medium, hard};
    public static float EasyMultiplier = 0.75f;
    public static float MediumMultiplier = 1f;
    public static float HardMultiplier = 1.5f;

    void Awake() {
        if (instance) {
            Destroy(gameObject);
        } else {
            Debug.Log("new settings");
            instance = this;
            instance.difficulty = GameDifficulty.medium;
        }
    }
     
    public static void SetDifficultyEasy() {
        Debug.Log("change to easy");
        instance.difficulty = GameDifficulty.easy;
    }

    public static void SetDifficultyMedium() {
        Debug.Log("change to medium");
        instance.difficulty = GameDifficulty.medium;
    }

    public static void SetDifficultyHard() {
        Debug.Log("change to hard");
        instance.difficulty = GameDifficulty.hard;
    }

    public static void SetDifficulty(int difficulty) {
        switch(difficulty) {
            case 0:
                SetDifficultyEasy();
                break;
            case 1:
                SetDifficultyMedium();
                break;   
            case 2:
                SetDifficultyHard();
                break;   
            default:
                SetDifficultyMedium();
                break;         
        }
    }

    public float GetMultiplier() {
        switch(instance.difficulty) {
            case GameDifficulty.easy:
                return EasyMultiplier;
            case GameDifficulty.medium:
                return MediumMultiplier;
            case GameDifficulty.hard:
                return HardMultiplier;
            default:
                return MediumMultiplier;
        }
    }
}
