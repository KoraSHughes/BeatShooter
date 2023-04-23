using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    public GameObject background;
    public GameObject startButton;
    public GameObject pauseButton;
    public GameObject settingsButton;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;
    public GameObject songSelectMenu;
    
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
        songSelectMenu.SetActive(false);
    }

    public void Update(){



#if !UNITY_WEBGL  // for webGL need platform conditional because it will just freeze otherwise
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
#endif
    }

    public void PlayGame() {
        SceneManager.LoadScene(1);
        background.SetActive(true);
        startButton.SetActive(false);
        pauseButton.SetActive(false);
        settingsButton.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        songSelectMenu.SetActive(true);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
        background.SetActive(true);
        startButton.SetActive(true);
        pauseButton.SetActive(false);
        settingsButton.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        songSelectMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PlaySongOne() {
        SceneManager.LoadScene(2);
        LevelUISettings();
    }


    public void PlaySongTwo() {
        SceneManager.LoadScene(3);
        LevelUISettings();
    }

    public void PlaySongThree() {
        SceneManager.LoadScene(4);
        LevelUISettings();
    }

    public void Next() {
        
    }

    public void LevelUISettings() {
        background.SetActive(false);
        // startButton.SetActive(true);
        pauseButton.SetActive(true);
        settingsButton.SetActive(false);
        // pauseMenu.SetActive(false);
        // settingsMenu.SetActive(false);
        // gameOverMenu.SetActive(false);
        songSelectMenu.SetActive(false);
        // IO(false, "background");
        // IO(false, "songSelectMenu");
        // IO(false, "settingsMenu");
        // IO(true, "pauseButton");
        // IO(false, "settingsButton");
    }

    private void Start(){
        scoreUI.text = "Score: " + score;
    }

    public void AddScore(int points){
        score += points;
        scoreUI.text = "Score: " + score;
    }

    public void GameOver(){
        // titleUI.text = "GAME OVER!!";
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
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

    public void PauseMenu() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PauseCloseButton() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        Time.timeScale = 1f;
    }

}

