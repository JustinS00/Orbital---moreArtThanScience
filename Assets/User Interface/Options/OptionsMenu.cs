using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static OptionsMenu instance;
    public GameDifficulty difficulty;
    public enum GameDifficulty {easy, medium, hard};
    public AudioMixer audioMixer;
    public static float EasyMultiplier = 0.50f;
    public static float MediumMultiplier = 1.0f;
    public static float HardMultiplier = 1.5f;
    public static int EasyHostileMobCap = 3;
    public static int MediumHostileMobCap = 4;
    public static int HardHostileMobCap = 5;
    private Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    void Awake() {
        if (instance) {
            Destroy(gameObject);
        } else {
            Debug.Log("new settings");
            instance = this;
            instance.difficulty = GameDifficulty.medium;
            gameObject.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int currentRes = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentRes = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentRes;
        resolutionDropdown.RefreshShownValue();
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

    public int GetHostileMobCap() {
        switch(instance.difficulty) {
            case GameDifficulty.easy:
                return EasyHostileMobCap;
            case GameDifficulty.medium:
                return MediumHostileMobCap;
            case GameDifficulty.hard:
                return HardHostileMobCap;
            default:
                return MediumHostileMobCap;
        }
    }

    public void SetMusicVolume(float volume) {
        audioMixer.SetFloat("Music", volume);
    }

    public void SetSoundVolume(float volume) {
        audioMixer.SetFloat("Sound", volume);
    }

    public void SetQuality(int index) {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetResolution(int index) {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
