using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D _rigidbody2D;
    public bool color = true;
    public GameObject explo1;
    public GameObject explo2;
    public GameObject _shield;
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
        _shield.GetComponent<Shield>().update_type(color);

        //health = Random.Range(minHealth, maxHealth+1);
        //healthBar.maxValue = health;
    }

    void Update() {
        //healthBar.value = health;
        if (health <= 1){
            // _shield.SetActive(false);
            _shield.GetComponent<Shield>().invis(true);
        }
        updatePos();
    }

    public void Initialize(float startPos, float endPos, float beat, float track, float color, float enemType){
        this.startPos = startPos;
        this.endPos = endPos;
        this.beat = beat;
        this.track = Mathf.RoundToInt(track);
        this.color = (color == 0) ? true : false;
        this.health = Mathf.RoundToInt(enemType + 1);
        //wasTriggered = false;
        
        _conductor = GameObject.Find("Conductor").GetComponent<Conductor>();
    }

    private void updatePos() {
        if (track == 0) { //top
            transform.position = new Vector3(transform.position.x,
                                            startPos + (2) * (1f + (beat - _conductor.songPosInBeats) * 2f),
                                            transform.position.z);
            //print(transform.position.x + " " + (startPos + (endPos - startPos) * (1f - (beat - _conductor.songPosInBeats))) + " " + transform.position.z);
        }
        else if (track == 1) { //left
            transform.position = new Vector3(startPos + (2) * (1f - (beat - _conductor.songPosInBeats) * 2f),
                                            transform.position.y,
                                            transform.position.z);
            //print((startPos + (endPos - startPos) * (1f - (beat - _conductor.songPosInBeats))) + " " + transform.position.y + " " + transform.position.z);
        }
        else if (track == 2) { //bottom
            transform.position = new Vector3(transform.position.x,
                                            startPos + (2) * (1f - (beat - _conductor.songPosInBeats) * 2f),
                                            transform.position.z);
        }
        else if (track == 3) { //right
            transform.position = new Vector3(startPos + (2) * (1f + (beat - _conductor.songPosInBeats) * 2f),
                                            transform.position.y,
                                            transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if ( (other.CompareTag("bullet1") && color == true) || (other.CompareTag("bullet2") && color == false) ) {
            // _gameManager.AddScore(eVal);
            health -= 1;
            isHit = true;
            if (health == 0){
                Instantiate((color)?explo1:explo2, transform.position, Quaternion.identity);
                Destroy(gameObject);
                _gameManager.AddScore(10);
            }
            else{
                Instantiate(smallexplo, transform.position, Quaternion.identity);
                if (health == 1){
                    _shield.GetComponent<Shield>().invis(true);
                }
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player")){
            if (other.GetComponent<Player>().isShielded() == false){
                print("hit player");
                bool isAlive = other.GetComponent<Player>().damage(1);
                if (isAlive){
                    print("alive");
                    Instantiate((color)?explo1:explo2, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                else {
                    print("dead");
                    Instantiate(bigexplo, other.transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                    _gameManager.GameOver();
                }
            }
        }
        else if (other.CompareTag("shield")){
            print("hit shield");
            if (health < 0) {
                _gameManager.AddScore(10);
            }
            health = 0;
            Instantiate((color)?explo1:explo2, transform.position, Quaternion.identity);
            Destroy(gameObject);
            _gameManager.AddScore(10);
        }
    }
}
