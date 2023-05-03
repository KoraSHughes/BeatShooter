using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D _rigidbody2D;
    public bool type = true;
    public GameObject explo1;
    public GameObject explo2;
    public GameObject Shield;
    public GameObject smallexplo;
    public GameObject bigexplo;
    GameManager _gameManager;

    // int eVal = 5;
    // GameManager _gameManager;
    int health = 1;
    public int maxHealth = 2;
    public int minHealth = 1;

    public float startPos, endPos;
    public int track; //0 = w; 1 = a; 2 = s; 3 = d
    public float beat;
    public bool isHit;

    public Conductor _conductor;
    //public Slider healthBar;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        Shield.GetComponent<Shield>().update_type(type);

        health = Random.Range(minHealth, maxHealth+1);
        //healthBar.maxValue = health;
        if (health <= 1){
            // Shield.SetActive(false);
            Shield.GetComponent<Shield>().invis(true);
        }
    }

    void Update() {
        //healthBar.value = health;
        updatePos();
    }

    public void Initialize(float startPos, float endPos, float beat, float track, float enemType){
        this.startPos = startPos;
        this.endPos = endPos;
        this.beat = beat;
        this.track = Mathf.RoundToInt(track);
        this.type = (enemType == 1) ? true : false;
        //wasTriggered = false;
        
        _conductor = GameObject.Find("Conductor").GetComponent<Conductor>();
    }

    private void updatePos() {
        if ((track == 0) || (track == 2)) { //top & bottom
            transform.position = new Vector3(transform.position.x,
                                            startPos + (endPos - startPos) * (1f - (beat - _conductor.songPosInBeats)),
                                            transform.position.z);
            print("startPos: " + startPos + " endPos: " + endPos);
            print("math: " + (endPos - startPos) * (1f - (beat - _conductor.songPosInBeats)));
            print(transform.position.x + " " + (startPos + (endPos - startPos) * (1f - (beat - _conductor.songPosInBeats))) + " " + transform.position.z);
        }
        else if ((track == 1) || (track == 3)) { //left & right
            transform.position = new Vector3(startPos + (endPos - startPos) * (1f - (beat - _conductor.songPosInBeats)),
                                            transform.position.y,
                                            transform.position.z);
            print("startPos: " + startPos + " endPos: " + endPos);
            print("math: " + (endPos - startPos) * (1f - (beat - _conductor.songPosInBeats)));
            print((startPos + (endPos - startPos) * (1f - (beat - _conductor.songPosInBeats))) + " " + transform.position.y + " " + transform.position.z);
        }
/*         else if (track == 2) { //bottom
            transform.position = new Vector3(transform.position.x,
                                            startPos + (endPos - startPos) * (1f - (beat - _conductor.songPosInBeats) * 2f),
                                            transform.position.z);
        }
        else if (track == 3) { //right
            transform.position = new Vector3(startPos + (endPos - startPos) * (1f - (beat - _conductor.songPosInBeats) * 2f),
                                            transform.position.y,
                                            transform.position.z);
        } */
    }

    private void OnTriggerEnter2D(Collider2D other){
        if ( (other.CompareTag("bullet1") && type == true) || (other.CompareTag("bullet2") && type == false) ) {
            // _gameManager.AddScore(eVal);
            health -= 1;
            isHit = true;
            if (health <= 0){
                Instantiate((type)?explo1:explo2, transform.position, Quaternion.identity);
                Destroy(gameObject);
                _gameManager.AddScore(10);
            }
            else{
                Instantiate(smallexplo, transform.position, Quaternion.identity);
                if (health == 1){
                    Shield.GetComponent<Shield>().invis(true);
                }
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player")){
            if (other.GetComponent<Player>().isShielded() == false){
                bool isAlive = other.GetComponent<Player>().damage(1);
                if (isAlive){
                    Instantiate((type)?explo1:explo2, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                else{
                    Instantiate(bigexplo, other.transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                    _gameManager.GameOver();
                }
            }
        }
        // else if (other.CompareTag("Shield")){ 
        //     health = 0;
        //     Instantiate((type)?explo1:explo2, transform.position, Quaternion.identity);
        //     Destroy(gameObject);
        //     _gameManager.AddScore(10);
        // }
    }
}
