using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float[] eSpeed = {240f, 240f, 80f, 80f};

    public GameObject top;
    public GameObject bottom;
    public GameObject left;
    public GameObject right;

    public GameObject[] enemies;

    float mytime = 0;

    public TextAsset map = null;
    private float songBpm;
    private float secPerBeat;
    private float firstBeatOffset;
    private static float songPos;
    private static float songPosInBeats;
    private float dspSongTime;


    private string level = "w.......a.......s.......d..............";
    private string enemyType = "";

    private float bps = 4;
    int i = 0;  // position in level

    void Start() {
        if (map == null){  // no map selected
            level = randomLevel();
            enemyType = randomEtypes();
        }
        else{
            mapToLevel();
            enemyType = randomEtypes();  //change if we dont want random colors
        }

        if (level.Length != enemyType.Length){  // double check
            Debug.LogError("Level & Enemy Type Definition Incongruent\n"+level+"\n"+enemyType);
        }
    }

    void Update() {  // run map
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        
        mytime += Time.deltaTime;

        if (i < level.Length && mytime >= (1/bps)){
            if (level[i] == '.'){
                // skip
            }
            else{
                int eType = enemyType[i] - '0';  // int.Parse(enemyType[i]);
                if (eType < 0 || eType >= enemies.Length){
                    Debug.LogError("ENEMY TYPE INCORRECT: " + eType.ToString());
                }

                if (level[i] == 'w'){
                    spawnTop(eType);
                }
                else if (level[i] == 'a'){
                    spawnLeft(eType);
                }
                else if (level[i] == 's'){
                    spawnBottom(eType);
                }
                else if (level[i] == 'd'){
                    spawnRight(eType);
                }
                else{
                    Debug.Log("Error on Level String: " + level[i]);
                }
            }
            mytime = 0;
            i += 1;
        }
        else{
            //WIN!
        }
    }

    private void spawnLeft(int etype){
        GameObject newEnemy = Instantiate(enemies[etype], left.transform.position, Quaternion.identity);
        // rotate to face right
        newEnemy.transform.localRotation = Quaternion.Euler(0, 0, -90);
        newEnemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(eSpeed[etype], 0));
    }
    private void spawnRight(int etype){
        GameObject newEnemy = Instantiate(enemies[etype], right.transform.position, Quaternion.identity);
        // rotate to face left
        newEnemy.transform.localRotation = Quaternion.Euler(0, 0, 90);
        newEnemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(-eSpeed[etype], 0));
    }
    private void spawnTop(int etype){
        GameObject newEnemy = Instantiate(enemies[etype], top.transform.position, Quaternion.identity);
        // rotate to face down
        newEnemy.transform.localRotation = Quaternion.Euler(0, 0, 180);
        newEnemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -eSpeed[etype]/2.2f));
    }
    private void spawnBottom(int etype){
        GameObject newEnemy = Instantiate(enemies[etype], bottom.transform.position, Quaternion.identity);
        newEnemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, eSpeed[etype]/2.2f));
    }


    private int randType(){
        return Random.Range(0, enemies.Length);
    }
    string directions = "wasd";
    private string randomLetter(){
        int num = Random.Range(0,directions.Length);
        return directions[num]+"";
    }
    private string randomPause(int num){
        string retstring = " ";
        for (int i = 0; i < Random.Range(0, num); i++) {
            retstring += ".";
        }
        return retstring;
    }

    private string randomLevel(){
        Debug.Log("**GENERATING RANDOM LEVEL**");
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

    private string randomEtypes(){
        string outTypes = "";
        for (int i=0; i < level.Length; i++){
            if (level[i] == '.'){
                outTypes += ".";
            }
            else{
                outTypes += randType();
            }
        }
        return outTypes;
    }

    //private float beatDiv = 16.0f;  // 16th notes
    // private void mapToLevel() {
    //     Debug.Log("*reading map file...");
    //     string fs = map.txt;
    //     string[] maplines = fs.Split('\n');

    //     level = "";
    //     for(size_t i = 0; i < maplines.length; ++i) {
    //         level += maplines[i];
    //     }
        
    //     Debug.Log("...map finished parsing");
    //     level = outNotes;  // write to globals
    //     enemyType = outEtypes;
    // }

    private void mapToLevel() {
        Debug.Log("*reading map file...");
        string fs = map.text;
        string[] maplines = fs.Split('\n');

        level = "";
        for(int i = 0; i < maplines.Length; ++i) {
            level += maplines[i];
        }
        Debug.Log(level);
        Debug.Log("...map finished parsing");
        //level = outNotes;  // write to globals
        //enemyType = outEtypes;
    }

/*     private void mapToLevel(){
        Debug.Log("*reading map file...");
        string outNotes = "";
        string outEtypes = "";
        string fs = map.text;
        string[] maplines = fs.Split('\n');

        var metadata = maplines[0].Split(' ');
        Debug.Log(metadata[0] + " " + metadata[1] + " " + metadata[2]);

        songBpm = float.Parse(metadata[0].Substring(1));
        bps = songBpm*60.0f*beatDiv;  // bps * note subdivision: this makes sure that notes must fall on beat
        Debug.Log("songBpm:" + songBpm);
        
        // endMarker = float.Parse(metadata[2]);
        // Debug.Log(endMarker);

        float offset = float.Parse(metadata[1]);
        for(int i=0; i < (int)(offset*bps); i++){
            outNotes += '.';
        }
        Debug.Log("songOffset:" + offset);

        float last_time = 0f;
        for(int i=1; i<maplines.Length; i++){
            string[] noteInfo = maplines[i].Split(' ');
            if (noteInfo[0].Contains("/") || noteInfo.Length==1)
                continue; // skip comments

            // adding time spacing
            float noteTime = float.Parse(noteInfo[2]);
            int timing = (int)Mathf.Round((noteTime-last_time)*bps);
            for(int j=0; j < timing; j++){
                outNotes += ".";
                outEtypes += ".";
            }
            last_time = noteTime; // update the global time of the last note for the next note

            // adding enemy type info
            string noteType = noteInfo[1];
            switch (noteType){
                case "COLORA":
                    outEtypes += 'a';
                    break;
                case "COLORB":
                    outEtypes += 'b';
                    break;
                default:
                    Debug.Log("wrong note type read: " + noteType);
                    // outEtypes += '.';
                    break;
            }

            // append actual note
            if ("wasd".Contains(noteInfo[0])){
                outNotes += noteInfo[0];
            }
            else {
                Debug.Log("Actual note not readable?: " + noteInfo[0]);
                // outNotes += '.';
            }
        }
        Debug.Log("...map finished parsing");
        level = outNotes;  // write to globals
        enemyType = outEtypes;
    } */
}
