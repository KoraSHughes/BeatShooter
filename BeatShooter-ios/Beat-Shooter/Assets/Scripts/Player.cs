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

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        color1 = bulletPrefab1.GetComponent<SpriteRenderer>().color;
        color2 = bulletPrefab2.GetComponent<SpriteRenderer>().color;
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
        // float xSpeed = Input.GetAxis("Horizontal") * pSpeed;
        // float ySpeed = Input.GetAxis("Vertical") * pSpeed;
        // _rigidbody2D.velocity = new Vector2(xSpeed, ySpeed);

        // transform.position = new Vector2(0f, 0f);
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");
        if (horz < 0){  // Input.GetButtonDown("left")
            myAngle = 90f;
        }
        else if (horz > 0){
            myAngle = -90f;
        }
        else if (vert > 0){
            myAngle = 0f;
        }
        else if (vert < 0){
            myAngle = 180f;
        }
        if (vert != 0 || horz != 0){
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
}
