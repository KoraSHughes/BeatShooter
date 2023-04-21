using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame() {
        SceneManager.LoadScene(1);   
        gameManager.IO(true, "songSelectMenu");
        gameManager.IO(true, "background");
        gameManager.IO(false, "startButton");
        gameManager.IO(true, "songSelectMenu");
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
        gameManager.IO(true, "background");
        gameManager.IO(true, "startButton");
        gameManager.IO(false, "songSelectMenu");
    }

    public void PlaySongOne() {
        SceneManager.LoadScene(2);
        gameManager.IO(false, "background");
        gameManager.IO(false, "songSelectMenu");
        gameManager.IO(false, "settingsMenu");
        
    }

    public void PlaySongTwo() {
        SceneManager.LoadScene(3);
        gameManager.IO(false, "background");
        gameManager.IO(false, "songSelectMenu");
        gameManager.IO(false, "settingsMenu");
    }

    public void PlaySongThree() {
        SceneManager.LoadScene(4);
        gameManager.IO(false, "background");
        gameManager.IO(false, "songSelectMenu");
        gameManager.IO(false, "settingsMenu");
    }

    public void Restart() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
