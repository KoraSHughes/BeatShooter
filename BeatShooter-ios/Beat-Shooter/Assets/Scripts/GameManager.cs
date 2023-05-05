using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject shieldCooldownUI;
    // private GameObject heart1border;
    // private GameObject heart2border;
    // private GameObject heart3border;    
    public TextMeshProUGUI scoreUI;
    public GameObject scoreGameObject;
    public TextMeshProUGUI titleUI;
    public TextMeshProUGUI gameOverScoreText;

    private Image heart1image;
    private Image heart2image;
    private Image heart3image;
    private Image cooldown;
    private Image _shield;
    private Color blue = new Color(0, 0.3176471f, 0.7490196f);
    private Color red = new Color(0.7490196f, 0, 0.1882353f);

    public GameState state;
    
    private AudioSource backgroundMusic;

    private void Awake(){
        if (GameObject.FindObjectsOfType<GameManager>().Length > 1){
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }

        // heart1 = GameObject.FindGameObjectWithTag("Heart1");
        // heart2 = GameObject.FindGameObjectWithTag("Heart2");
        // heart3 = GameObject.FindGameObjectWithTag("Heart3");
        
        heart1image = heart1.GetComponent<Image>();
        heart2image = heart2.GetComponent<Image>();
        heart3image = heart3.GetComponent<Image>();

        cooldown = GameObject.FindGameObjectWithTag("Cooldown").GetComponent<Image>();
        _shield = GameObject.FindGameObjectWithTag("ShieldUI").GetComponent<Image>();
        
        // heart1border = GameObject.FindGameObjectWithTag("Heart1Border");
        // heart2border = GameObject.FindGameObjectWithTag("Heart2Border");
        // heart3border = GameObject.FindGameObjectWithTag("Heart3Border");

        background.SetActive(true);
        startButton.SetActive(true);
        pauseButton.SetActive(false);
        settingsButton.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        songSelectMenu.SetActive(false);
        HealthUI(false);
        scoreGameObject.SetActive(false);
        shieldCooldownUI.SetActive(false);
        backgroundMusic = GetComponent<AudioSource>();
        backgroundMusic.Play();
    }

    public void Update(){
        if (Player.health == 2) {
            heart3.SetActive(false);
        }
        else if (Player.health == 1) {
            heart2.SetActive(false);
        }
        else if (Player.health == 0) {
            heart1.SetActive(false);
        }


#if !UNITY_WEBGL  // for webGL need platform conditional because it will just freeze otherwise
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
#endif
    }

    public void UpdateGameState(GameState newState) {
        
    }

    public void PlayGame() {
        SceneManager.LoadScene(1);
        background.SetActive(true);
        startButton.SetActive(false);
        pauseButton.SetActive(false);
        settingsButton.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        songSelectMenu.SetActive(true);
        HealthUI(false);
        scoreGameObject.SetActive(false);
        shieldCooldownUI.SetActive(false);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
        background.SetActive(true);
        startButton.SetActive(true);
        pauseButton.SetActive(false);
        settingsButton.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        songSelectMenu.SetActive(false);
        HealthUI(false);
        scoreGameObject.SetActive(false);
        shieldCooldownUI.SetActive(false);
        Time.timeScale = 1f;
        AddScore(-score);
        backgroundMusic.Play();
    }

    public void Tutorial() {
        SceneManager.LoadScene(5);
        background.SetActive(false);
        startButton.SetActive(false);
        settingsButton.SetActive(false);
        HealthUI(false);
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
        HealthUI(true);
        HealthUIColor("red");
        Player.health = 3;
        scoreGameObject.SetActive(true);
        shieldCooldownUI.SetActive(true);
        backgroundMusic.Pause();
    }

    private void Start(){
        scoreUI.text = "Score:" + score;
    }

    public void AddScore(int points){
        score += points;
        scoreUI.text = "Score:" + score;
    }

    public void GameOver(){
        // titleUI.text = "GAME OVER!!";
        gameOverMenu.SetActive(true);
        PauseHandler();
        
        gameOverScoreText.text = "Score:" + score;
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

    public void PauseHandler() {
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.Gameplay
            ? GameState.Paused
            : GameState.Gameplay;

        GameStateManager.Instance.SetState(newGameState);
    }

    public void PauseMenu() {
        pauseMenu.SetActive(true);
        PauseHandler();
        Time.timeScale = 0f;
    }

    public void PauseCloseButton() {
        pauseMenu.SetActive(false);
        PauseHandler();
        Time.timeScale = 1f;
    }

    public void Restart() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        HealthUI(true);
        Time.timeScale = 1f;
        Player.health = 3;
        AddScore(-score);
    }

    public void HealthUI(bool io) {
        // print("healthui");
        heart1.SetActive(io);
        heart2.SetActive(io);
        heart3.SetActive(io);
        // heart1border.SetActive(io);
        // heart2border.SetActive(io);
        // heart3border.SetActive(io);
    }

    public void HealthUIColor(string color) {
        if (color == "blue") {   // blue
            heart1image.color = blue;
            heart2image.color = blue;
            heart3image.color = blue;
            cooldown.color = blue;
            _shield.color = blue;
        } else {
            heart1image.color = red;
            heart2image.color = red;
            heart3image.color = red;
            cooldown.color = red;
            _shield.color = red;
        }
    }
}

