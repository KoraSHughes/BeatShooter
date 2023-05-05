using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    bool type = true;
    private Color blueColor = new Color(0, 0.3176471f, 0.7490196f);
    private Color redColor = new Color(0.7490196f, 0, 0.1882353f);
    //Color notSeen = new Color(0,0,0,0);

    // Start is called before the first frame update
    void Start() {
        gameObject.SetActive(false);
        update_color();
    }

    void update_color() {
        //print("color: " + (type ? "blue" : "red"));
        transform.GetComponent<SpriteRenderer>().color = type ? blueColor : redColor;
    }

    public void update_type(bool newType) {
        type = newType;
        update_color();
    }

    public void invis(bool isInvis) {
        gameObject.SetActive(!isInvis);
    }

    public void ActiveSetter(bool val) {
        gameObject.SetActive(val);
    }
}