using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	Rigidbody2D _rigidbody2D;
    SpriteRenderer sprite;
    public Transform spawnPoint;

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

    private float width;
    private float height;
    float touch_threshold = 0.4f;

    public int health = 2;  // number of hits player can take

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        color1 = bulletPrefab1.GetComponent<SpriteRenderer>().color;
        color2 = bulletPrefab2.GetComponent<SpriteRenderer>().color;

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToShoot > 0){ // shoot cooldown
            timeToShoot -= Time.deltaTime;
        }
        else{
            timeToShoot = 0;
        }
        

        for (int i=0; i < Input.touchCount; ++i){  // track all touches
            Touch touch = Input.GetTouch(i);
            Vector2 pos = cam.ScreenToWorldPoint(touch.position); 

            if (i == 0 && touch.phase == TouchPhase.Began){
                Debug.Log("Touches " + Input.touchCount.ToString() + ": " + pos.ToString());
                int touch_reigon = get_reigon(pos);
                
                // rotate user based on reigon
                switch (touch_reigon)
                {
                    case 1:
                        myAngle = 90f;
                        break;
                    case 2:
                        myAngle = -90f;
                        break;
                    case 3:
                        myAngle = 0f;
                        break;
                    case 4:
                        myAngle = 180f;
                        break;
                    default:
                        myAngle = defaultAng;
                        break;
                }
                // shoot
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
                        if (myAngle == 180){    // facing down
                            newBullet.transform.localRotation = Quaternion.Euler(0, 0, 180);
                            newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -bulletSpeed));
                        }
                        else if (myAngle == 0){ // facing up
                            newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bulletSpeed));
                        }
                        else if (myAngle == -90){   // facing right
                            newBullet.transform.localRotation = Quaternion.Euler(0, 0, -90);
                            newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletSpeed, 0));
                        }
                        else{   // facing left
                            newBullet.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-bulletSpeed, 0));
                        }
                        timeToShoot = 0.1f;
                    }
                }
                else{
                     // swap guns
                    if (gunType){
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
    }

    int get_reigon(Vector2 touchPos){
        // returns screen reigon {left: 1, right: 2, up: 3, down: 4} & 0 is not far enough
        if (Mathf.Abs(touchPos.x) < width*touch_threshold ||
            Mathf.Abs(touchPos.y) < height*touch_threshold) {
                return 0;
        }
        else{
            float diff = Mathf.Abs(touchPos.x) - Mathf.Abs(touchPos.y);
            if (diff > 0){  // x movement
                return (touchPos.x > 0) ? 2 : 1;
            }
            else{
                return (touchPos.y > 0) ? 3 : 4;
            }
        }
    }

    string show_vecs(Vector2[] vecs){
        string ret = "[ ";
        for (int i=0; i < vecs.Length; ++i){
            ret += vecs[i].ToString() + " ";
        }
        ret += "]";
        return ret;
    }

    public bool damage(int new_damage){
        health -= new_damage;
        return health > 0;
    }
}