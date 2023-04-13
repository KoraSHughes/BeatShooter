using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float eSpeed = 200f;

    public GameObject top;
    public GameObject bottom;
    public GameObject left;
    public GameObject right;

    public GameObject enemy1;
    public GameObject enemy2;

    float mytime = 0;

    private string level = "w.......a.......s.......d..............";
    private float bps = 8;

    int i = 0;
    bool etype = true;

    void Start()
    {
        level = generateLevel();
        Debug.Log("LEVEL: " + level);
    }
    // Update is called once per frame
    void Update()
    {
        mytime += Time.deltaTime;

        if (i < level.Length && mytime >= (1/bps)){
            if (level[i] == '.'){
                // skip
                // Debug.Log("Skipping beat");
            }
            else if (level[i] == 'w'){
                spawnTop(randomBoolean());
            }
            else if (level[i] == 'a'){
                spawnLeft(randomBoolean());
            }
            else if (level[i] == 's'){
                spawnBottom(randomBoolean());
            }
            else if (level[i] == 'd'){
                spawnRight(randomBoolean());
            }
            else{
                Debug.Log("Error on Level String: " + level);
            }
            mytime = 0;

            if (etype == true){
                etype = false;
            }
            else{
                etype = true;
            }
            i += 1;
        }
        else{
            //WIN!
        }
    }

    private void spawnLeft(bool etype){
        GameObject enemy = enemy2;
        if (etype){
            enemy = enemy1;
        }
        GameObject newEnemy = Instantiate(enemy, left.transform.position, Quaternion.identity);
        newEnemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(eSpeed, 0));
    }
    private void spawnRight(bool etype){
        GameObject enemy = enemy2;
        if (etype){
            enemy = enemy1;
        }
        GameObject newEnemy = Instantiate(enemy, right.transform.position, Quaternion.identity);
        newEnemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(-eSpeed, 0));
    }
    private void spawnTop(bool etype){
        GameObject enemy = enemy2;
        if (etype){
            enemy = enemy1;
        }
        GameObject newEnemy = Instantiate(enemy, top.transform.position, Quaternion.identity);
        newEnemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -eSpeed/2));
    }
    private void spawnBottom(bool etype){
        GameObject enemy = enemy2;
        if (etype){
            enemy = enemy1;
        }
        GameObject newEnemy = Instantiate(enemy, bottom.transform.position, Quaternion.identity);
        newEnemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, eSpeed/2));
    }

    private bool randomBoolean(){
        float randnum = Random.Range(0, 10);
        if (randnum >= 5)
        {
            return true;
        }
        return false;
    }
    private string randomLetter(){
        bool a = randomBoolean();
        bool b = randomBoolean();
        if (a) {
            if (b){
                return "w";
            }
            else{
                return "a";
            }
        }
        else{
            if (b){
                return "s";
            }
            else{
                return "d";
            }
        }
    }
    private string randomPause(int num){
        string retstring = " ";
        for (int i = 0; i < Random.Range(0, num); i++) {
            retstring += ".";
        }
        return retstring;
    }

    private string generateLevel(){
        string retstring = "";
        for (int i = 0; i < 8; i++) {
            retstring += randomLetter() + "..........";
        }
        retstring += "........";
        for (int i = 0; i < 8; i++) {
            retstring += randomLetter() + "......." + randomPause(3);
        }
        retstring += "........";
        for (int i = 0; i < 8; i++) {
            retstring += randomLetter() + "....." + randomPause(2);
        }
        retstring += "........";
        for (int i = 0; i < 8; i++) {
            retstring += randomLetter() + "....." + randomPause(4);
        }
        retstring += "........";
        for (int i = 0; i < 8; i++) {
            retstring += randomLetter() + "...." + randomPause(4);
        }
        retstring += "........";
        for (int i = 0; i < 8; i++) {
            retstring += randomLetter() + ".." + randomPause(5);
        }
        retstring += "........";
        for (int i = 0; i < 8; i++) {
            retstring += randomLetter() + "....";
        }
        retstring += "........";
        for (int i = 0; i < 8; i++) {
            retstring += randomLetter() + ".." + randomPause(3);
        }
        retstring += "...........";
        for (int j = 7; j > 0; j--) {
            retstring += ".....";
            for (int i = 0; i < bps*10; i++) {
                retstring += randomLetter() + randomPause(j);
            }
        }
        return retstring;
    }
}
