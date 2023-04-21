using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D _rigidbody2D;
    public bool type = true;
    public GameObject explo1;
    public GameObject explo2;
    public GameObject explo3;

    int eVal = 5;

    GameManager _gameManager;


    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
 

    }



    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("bullet1") && type == true){
            _gameManager.AddScore(eVal);
            Instantiate(explo1, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("bullet2") && type == false){
            _gameManager.AddScore(eVal);
            Instantiate(explo2, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player")){
            _gameManager.GameOver();
            Instantiate(explo3, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}
