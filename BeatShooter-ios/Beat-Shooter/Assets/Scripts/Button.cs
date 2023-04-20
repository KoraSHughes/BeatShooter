using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame() {
        SceneManager.LoadScene(1);       
    }

    public void PlaySongOne() {
        SceneManager.LoadScene(2);
    }

    public void PlaySongTwo() {
        SceneManager.LoadScene(3);
    }

    public void PlaySongThree() {
        SceneManager.LoadScene(4);
    }
}
