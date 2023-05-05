using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    //public float ShieldDuration = 1f;
    public float ShieldInc = 5f;
    private Slider shieldCooldown;

    Vector2 firstPressPos = new Vector2(-100,-100), currentSwipe = new Vector2(-100,-100), secondPressPos;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        sprite = GetComponent<SpriteRenderer>();
        color1 = bulletPrefab1.GetComponent<SpriteRenderer>().color;
        color2 = bulletPrefab2.GetComponent<SpriteRenderer>().color;

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        shieldCooldown = GameObject.FindGameObjectWithTag("ShieldCooldown").GetComponent<Slider>();
        _gameManager.HealthUIColor("blue");

        // width = (float)Screen.width / 2.0f;
        // height = (float)Screen.height / 2.0f;
    }

    void Update() {
        // if (SceneManager.GetActiveScene().name == "4) Song2") {
        //     _shield.SetActive(true);
        // }
        //print(health);
        shieldCooldown.value = 5 - timeToShield;
        //print(timeToShield);

        shieldAppear();

        // do a block/Shield
/*         if (timeToShield <= 0){
            
            timeToShield += ShieldInc + ShieldDuration;
        } */
        for (int i=0; i < Input.touchCount; ++i){
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began) {
                firstPressPos = touch.position;
                secondPressPos = touch.position;
            }
            if (touch.phase == TouchPhase.Ended) {
                firstPressPos = touch.position;
                bool val = true;
                if (timeToShield <= 0)
                    val = detectSwipe();
                if (!val)
                    singleTouch(touch);
            }
        }
    }

    public void shieldAppear() {
        if (timeToShoot > 0){ // shoot cooldown
            timeToShoot -= Time.deltaTime;
        }
        else{
            timeToShoot = 0;
        }
        
        if (timeToShield > 0){
            timeToShield -= Time.deltaTime;
            if (timeToShield >= ShieldInc) {
                _shield.GetComponent<Shield>().ActiveSetter(false);
            }
            else{
                _shield.GetComponent<Shield>().ActiveSetter(true);
            }
        }
        else{
            timeToShield = 0;
            _shield.GetComponent<Shield>().ActiveSetter(false);
        }
    }

    public void singleTouch(Touch touch) {
        Vector2 pos = cam.ScreenToWorldPoint(touch.position); 
        int touch_region = get_region(pos);

        //if (i == 0 && touch.phase == TouchPhase.Began){
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
        print("single touch; angle: " + myAngle);
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
        //}
        
    }

    public bool detectSwipe() {
        //if (Input.touches.Length > 0) {
        float minSwipeLength = 20f;
        currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
    
        // Make sure it was a legit swipe, not a tap
        if (currentSwipe.magnitude < minSwipeLength) {
            //singleTouch();
            return false;
        }
        print("swipe: " + Input.touches.Length);
        currentSwipe.Normalize();

        // Swipe up
        if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) 
            _shield.GetComponent<Shield>().ActiveSetter(true);
        // Swipe down
        else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            _shield.GetComponent<Shield>().ActiveSetter(true);
        // Swipe left
        else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            _shield.GetComponent<Shield>().ActiveSetter(true);
        // Swipe right
        else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            _shield.GetComponent<Shield>().ActiveSetter(true);
        //}
        return true;
    }

    int get_region(Vector2 touchPos){
        // returns screen region {left: 1, right: 2, up: 3, down: 4} & 0 is not far enough
        //Debug.Log("Comparing:" + (Mathf.Abs(touchPos.x),Mathf.Abs(touchPos.y)).ToString() +
                  //" vs. " + (width*touch_threshold, height*touch_threshold).ToString());
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