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
    public GameObject smallexplo;
    public GameObject bigexplo;

    // int eVal = 5;
    // GameManager _gameManager;
    public int health = 1;


    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        // _gameManager = GameObject.FindObjectOfType<GameManager>();
    }



    private void OnTriggerEnter2D(Collider2D other){
        if ( (other.CompareTag("bullet1") && type == true) || (other.CompareTag("bullet2") && type == false) ) {
            // _gameManager.AddScore(eVal);
            health -= 1;
            if (health <= 0){
                Instantiate((type)?explo1:explo2, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
            else{
                Instantiate(smallexplo, transform.position, Quaternion.identity);
            }
        }
        else if (other.CompareTag("Player")){
            // _gameManager.GameOver();
            bool isAlive = other.GetComponent<Player>().damage(1);
            if (isAlive){
                 Instantiate(smallexplo, other.transform.position, Quaternion.identity);
            } 
            else{
                Instantiate(bigexplo, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
            
        }
    }
}
