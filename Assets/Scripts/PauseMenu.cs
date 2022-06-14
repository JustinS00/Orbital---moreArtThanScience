using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool gameIsPausedFromMenu;
    private GameObject pauseMenuUI;
    private GameManager gameManager;
    private bool gameIsPausedFromOthers;

    // Update is called once per frame

    private void Awake() {
        pauseMenuUI = GameObject.Find("PauseMenu");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pauseMenuUI.SetActive(false);
        gameIsPausedFromMenu= false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPausedFromMenu) {
                Resume();
            } else {
                Pause();
            }
        }
    
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        TogglePause();
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        TogglePause();
    }

    public void ShowAchievement() {
        Achievement.ShowAchievement_Static();
    }
            
    private void TogglePause() {
        gameIsPausedFromMenu = !gameIsPausedFromMenu;
        if (gameIsPausedFromMenu != gameManager.isGamePaused())
            gameManager.TogglePause();
    }

    public void Menu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame() {
        Debug.Log("Quiting Game");
        Application.Quit();
    }

}
