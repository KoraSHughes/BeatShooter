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
    public GameObject smallexplo;
    public GameObject bigexplo;
    GameManager _gameManager;

    // int eVal = 5;
    // GameManager _gameManager;
    public int health = 1;
    public int track; //0 = w; 1 = a; 2 = s; 3 = d
    public float beat;
    public bool isHit;
    public Slider healthBar;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Update() {
        healthBar.value = health;
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
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player")){
            bool isAlive = other.GetComponent<Player>().damage(1);
            if (isAlive){
                 Instantiate(smallexplo, other.transform.position, Quaternion.identity);
            } 
            else{
                Instantiate(bigexplo, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                _gameManager.GameOver();
            }
            
        }
        else if (other.CompareTag("shield")){ 
            health = 0;
            Instantiate((type)?explo1:explo2, transform.position, Quaternion.identity);
            Destroy(gameObject);
            _gameManager.AddScore(10);
        }
    }
}
