using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
	public GameManager _gameManager;
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

    private float width = 11.1f; // 9.3
    private float height = 6.65f;  // 5
    float touch_threshold = 0.15f;

    public static int health = 3;  // number of hits player can take

    public GameObject _shield;
    float timeToShield = 0.0f;
    private float ShieldDuration = 5.0f;
    private float ShieldInc = 15.0f;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        sprite = GetComponent<SpriteRenderer>();
        color1 = bulletPrefab1.GetComponent<SpriteRenderer>().color;
        color2 = bulletPrefab2.GetComponent<SpriteRenderer>().color;

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // width = (float)Screen.width / 2.0f;
        // height = (float)Screen.height / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // if (SceneManager.GetActiveScene().name == "4) Song2") {
        //     _shield.SetActive(true);
        // }
        //print(health);

        if (timeToShoot > 0){ // shoot cooldown
            timeToShoot -= Time.deltaTime;
        }
        else{
            timeToShoot = 0;
        }

        if (timeToShield > 0){
            timeToShield -= Time.deltaTime;
            if (timeToShield >= ShieldInc){
                _shield.GetComponent<Shield>().invis(false);
            }
            else{
                _shield.GetComponent<Shield>().invis(true);
            }
        }
        else{
            timeToShield = 0;
            _shield.GetComponent<Shield>().invis(true);
        }
        

        for (int i=0; i < Input.touchCount; ++i){  // track all touches
            Touch touch = Input.GetTouch(i);
            Vector2 pos = cam.ScreenToWorldPoint(touch.position); 
            int touch_region = get_region(pos);

            if (i == 0 && touch.phase == TouchPhase.Began){
                // Debug.Log("Touches " + Input.touchCount.ToString() + ": " + pos.ToString());
                // int touch_region = get_region(pos);
                
                // rotate user based on region
                switch (touch_region) {
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
                        _shield.GetComponent<Shield>().update_type(gunType);
                        _gameManager.HealthUIColor("red");
                        
                    }
                    else{
                        gunType = true;
                        sprite.color = color1;
                        _shield.GetComponent<Shield>().update_type(gunType);
                        _gameManager.HealthUIColor("blue");
                    }
                }
            }
            else if (i == 1 && touch.phase == TouchPhase.Began){
                // do a block/Shield
                if (timeToShield == 0){
                    timeToShield += ShieldInc + ShieldDuration;
                }
            }
        }
    }

    int get_region(Vector2 touchPos){
        // returns screen region {left: 1, right: 2, up: 3, down: 4} & 0 is not far enough
        Debug.Log("Comparing:" + (Mathf.Abs(touchPos.x),Mathf.Abs(touchPos.y)).ToString() +
                  " vs. " + (width*touch_threshold, height*touch_threshold).ToString());
        if (Mathf.Abs(touchPos.x) < width*touch_threshold &&
            Mathf.Abs(touchPos.y) < height*touch_threshold) {
                return 0;
        }
        else{
            if (Mathf.Abs(touchPos.x) > Mathf.Abs(touchPos.y)){  // x movement
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

    public bool isShielded(){
        return timeToShield >= ShieldInc;
    }
}