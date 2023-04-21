using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    public GameObject background;
    public GameObject startButton;
    public GameObject pauseButton;
    public GameObject settingsButton;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject settingsMenu;
    
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI titleUI;
    
    private void Awake(){
        if (GameObject.FindObjectsOfType<GameManager>().Length > 1){
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }

        pauseButton.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void Update(){
#if !UNITY_WEBGL  // for webGL need platform conditional because it will just freeze otherwise
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
#endif
    }

    private void Start(){
        scoreUI.text = "Score: " + score;
    }

    public void AddScore(int points){
        score += points;
        scoreUI.text = "Score: " + score;
    }

    public void GameOver(){
        titleUI.text = "GAME OVER!!";
    }

    public void YouWin(){
        titleUI.text = "** You  Win **";
    }

    public void SettingsMenu() {
        settingsMenu.SetActive(true);
    }

    public void SettingsCloseButton() {
        settingsMenu.SetActive(false);
    }

    public void IO(bool io, string UI) {
        if (UI == "background") {
            background.SetActive(io);
        }
        else if (UI == "startbutton") {
            startButton.SetActive(io);
        }
    }

}

