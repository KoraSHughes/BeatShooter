using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI titleUI;
    
    private void Awake(){
        if (GameObject.FindObjectsOfType<GameManager>().Length > 1){
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }
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

    public void Update(){
#if !UNITY_WEBGL  // for webGL need platform conditional because it will just freeze otherwise
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
#endif
    }
}

