using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    bool type = true;
    Color redColor = new Color(255, 64, 64);
    Color blueColor = new Color(64, 87, 255);
    Color notSeen = new Color(0,0,0,0);

    // Start is called before the first frame update
    void Start() {
        update_color();
    }

    // Update is called once per frame
    void Update() {
        
    }
    void update_color() {
        transform.GetComponent<SpriteRenderer>().material.color = type ? blueColor : redColor;
    }

    public void update_type(bool newType) {
        type = newType;
        update_color();
    }

    public void invis(bool isInvis) {
        if (isInvis){
            transform.GetComponent<SpriteRenderer>().material.color = notSeen;
        }
        else{
            update_color();
        }
    }
}
