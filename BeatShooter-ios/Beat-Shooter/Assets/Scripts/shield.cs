using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    bool type = true;
    Color redColor = new Color(255, 0, 64, 255);
    Color blueColor = new Color(0, 107, 255, 255);
    Color notSeen = new Color(0,0,0,0);

    // Start is called before the first frame update
    void Start() {
        update_color();
    }

    void update_color() {
        //print("color: " + (type ? "blue" : "red"));
        gameObject.SetActive(true);
        transform.GetComponent<SpriteRenderer>().material.color = type ? blueColor : redColor;
    }

    public void update_type(bool newType) {
        type = newType;
        update_color();
    }

    public void invis(bool isInvis) {
        if (isInvis){
            //print("isInvis");
            // transform.GetComponent<SpriteRenderer>().material.color = notSeen;
            gameObject.SetActive(false);
        }
        else{
            update_color();
        }
    }
}