using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	Rigidbody2D _rigidbody2D;
    SpriteRenderer sprite;
    public Transform spawnPoint;

	// int pSpeed = 5;
    // Start is called before the first frame update
    bool gunType = true;
    float myAngle = 0f;

    float timeToShoot = 0;  // shooting cooldown time
    int bulletSpeed = 600;
    public GameObject bulletPrefab1;
    Color color1;
    public GameObject bulletPrefab2;
    Color color2;
    
    private float defaultAng = -1;

    Camera cam;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        color1 = bulletPrefab1.GetComponent<SpriteRenderer>().color;
        color2 = bulletPrefab2.GetComponent<SpriteRenderer>().color;

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToShoot > 0){
            timeToShoot -= Time.deltaTime;
        }
        else{
            timeToShoot = 0;
        }
        
        for (int i=0; i< Input.touchCount; ++i){
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began) {
                Vector2 pos = cam.ScreenToWorldPoint(touch.position);
                Debug.Log("Found Touch: " + pos.ToString() + " with #Fingers: " + Input.touchCount.ToString());
            }
        }

        myAngle = defaultAng;
        if (leftMove()){  // Input.GetButtonDown("left")
            myAngle = 90f;
        }
        else if (rightMove()){
            myAngle = -90f;
        }
        else if (upMove()){
            myAngle = 0f;
        }
        else if (downMove()){
            myAngle = 180f;
        }
        if (myAngle != defaultAng){
            if (timeToShoot == 0){
                // rotate
                transform.rotation = Quaternion.Euler(0f,0f,myAngle);

                // shoot
                GameObject bulletPrefab = bulletPrefab1;
                if (!gunType){
                    bulletPrefab = bulletPrefab2;
                }

                GameObject newBullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
                if (myAngle == 180){
                    newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -bulletSpeed));
                }
                else if (myAngle == 0){
                    newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bulletSpeed));
                }
                else if (myAngle == -90){
                    newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletSpeed, 0));
                }
                else{
                    newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-bulletSpeed, 0));
                }
                timeToShoot = 0.4f;
            }
        }
        

        if (Input.GetButtonDown("Jump")){  // change guns
            if (gunType){  // TODO: change color of guns
                gunType = false;
                sprite.color = color2;
            }
            else{
                gunType = true;
                sprite.color = color1;
            }
        }
    }

    // movement functions
    bool upMove(){
        float vert = Input.GetAxis("Vertical");
        return vert > 0;
    }
    bool downMove(){
        float vert = Input.GetAxis("Vertical");
        return vert < 0;
    }
    bool leftMove(){
        float horz = Input.GetAxis("Horizontal");
        return horz < 0;
    }
    bool rightMove(){
        float horz = Input.GetAxis("Horizontal");
        return horz > 0;
    }
}