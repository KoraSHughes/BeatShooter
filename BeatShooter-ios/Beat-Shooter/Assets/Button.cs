using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private GameManager _gameManager;
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    public void StartButton()
    {
        _gameManager.PlayGame();
    }

    public void Back() {
        _gameManager.MainMenu();
    }
}
