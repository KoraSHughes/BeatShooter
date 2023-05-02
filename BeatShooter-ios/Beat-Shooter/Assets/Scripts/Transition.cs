using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Transition : MonoBehaviour
{
    public Animator _animation;
    public int level;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0)) {
        //     FadeTransition(level);
        // }
    }

    public void FadeTransition(int level) {
        _animation.SetTrigger("fadeOut");
    }

    public void loadNextScene() {
        SceneManager.LoadScene(level);
    }
}
