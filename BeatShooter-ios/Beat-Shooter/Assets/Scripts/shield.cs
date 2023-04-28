using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnTriggerEnter2D(Collider2D other){
    //     if (other.CompareTag("bullet1") || other.CompareTag("bullet2")){ 
    //         Instantiate((type)?explo1:explo2, transform.position, Quaternion.identity);
    //         Destroy(gameObject);
    //         // _gameManager.AddScore(10);
    //     }
    // }
    public destroy_shield(){
        Destroy(gameObject);
    }
}
