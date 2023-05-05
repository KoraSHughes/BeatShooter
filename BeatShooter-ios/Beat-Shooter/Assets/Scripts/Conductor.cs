using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public GameObject playerGO;
    //public PlayerAnims playerAnims;
    //public CameraShake camshake;
    public AudioSource hitAudio;
    public GameObject recorder;
    public Recorder rStats;
    //public StageController sController;

    public float songBpm; //DynamicFight: 90; Bach: 110; 
    public float secPerBeat;
    public float songPos;
    public float songPosInBeats;
    public float dspSongTime;
    public float firstBeatOffSet;

    public AudioSource _audioSource;

    public List<Vector2> notes; //y is beat, x is position
    public List<Vector2> otherNoteInfo;
    public int nextIndex;
    public float beatsSpawned;
    public float songPlayOffset;

    public float hitAccuracy;

    public static Conductor instance;

    //public MusicNote notePrefab;
    //public MusicNote hazardPrefab;
    
    public GameObject top, bottom, left, right;
    public Enemy enemy1A, enemy1B, enemy2A, enemy2B;
    //private float eSpeed = 200f;

    public TextAsset map;

    [HideInInspector]
    List<Enemy> spawnedEnemies;
    int spawnedEnemiesInd;
    public int currentTrack = 0;
    int lastanim;
    public float enemyKillWindow = 0.06f;
    bool done;
    public int combo, score, targetScore;
    int scoreAmnt = 100;    
    float endTimer = 0.5f;
    float endMarker;
    //float hzMod = 1; // modifier for hitting hazards

    //float[] holdTimer = new float[]{0f, 0f};
    //float holdInterval = 0.07f;

    //public HoldGhost holdGhost;
    //Animation anim;

    void Awake() {
        instance = this;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        GameStateManager.Instance.SetState(GameState.Gameplay);
        var rObj = Instantiate(recorder);
        rStats = rObj.GetComponent<Recorder>();
        done = false;
    }

    void OnDestroy() {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    void Start() {
        _audioSource = GetComponent<AudioSource>();

        dspSongTime = (float)AudioSettings.dspTime;
        spawnedEnemies = new List<Enemy>();
        Invoke("StartMusic", songPlayOffset);
        GenerateNotes();
        secPerBeat = 60f / songBpm;
        nextIndex = 0;
        spawnedEnemiesInd = 0;
        score = 0;
        combo = 0;
        //GameObject.Find("EndScene").GetComponent<Canvas>().enabled = false;
    }
    
    void Update() {
        songPos = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffSet);
        songPosInBeats = songPos / secPerBeat;

        // SPAAAAAAAAAAAAAWN NOTES
        if(nextIndex < notes.Count && notes[nextIndex].y < songPosInBeats + beatsSpawned){
            //print(spawnpoint.position + " " + Quaternion.identity);
            Enemy _enemy = enemy1A; //small and red
            Transform pos = top.transform;
            float startPos = pos.position.y;
            float endPos = playerGO.transform.position.y;
            Vector2 force = new Vector2(0,0);
            
            //x = type; y = color.
            if((otherNoteInfo[nextIndex].x == 0) && (otherNoteInfo[nextIndex].y == 1)) // if small and blue
                _enemy = enemy1B;
            else if((otherNoteInfo[nextIndex].x == -2) && (otherNoteInfo[nextIndex].y == 0)) // if large and red
                _enemy = enemy2A;
            else if((otherNoteInfo[nextIndex].x == -2) && (otherNoteInfo[nextIndex].y == 1)) // if large and blue
                _enemy = enemy2B;

/*             if(notes[nextIndex].x == 0) {
                pos = top.transform;
                //force = new Vector2(0, -eSpeed/2);
            } */
            if(notes[nextIndex].x == 1) {
                pos = left.transform;
                startPos = pos.position.x;
                endPos = playerGO.transform.position.x;
                //force = new Vector2(eSpeed, 0);
            }
            else if(notes[nextIndex].x == 2) {
                pos = bottom.transform;
                startPos = pos.position.y;
                endPos = playerGO.transform.position.y;
                //force = new Vector2(0, eSpeed/2);
            }
            else if(notes[nextIndex].x == 3) {
                pos = right.transform;
                startPos = pos.position.x;
                endPos = playerGO.transform.position.x;
                //force = new Vector2(-eSpeed, 0);
            }

            Enemy newEnemy = Instantiate(_enemy, pos.position, Quaternion.identity);
            //newEnemy.GetComponent<Rigidbody2D>().AddForce(force);
            newEnemy.Initialize(startPos, endPos, notes[nextIndex].y, notes[nextIndex].x, otherNoteInfo[nextIndex].x, otherNoteInfo[nextIndex].y);
            spawnedEnemies.Add(_enemy);
            nextIndex++;
        }

        if (_audioSource.time<=0 && spawnedEnemies.Count>0) {
            if (endTimer > 0) {
                endTimer -= Time.deltaTime;
            }
            else {
                if (!done) {
                    print(rStats.highestCombo);
                    print((float)score * ((float)rStats.highestCombo / (float)spawnedEnemies.Count));
                    print(score);
                    score = score + (int)((float)score * ((float)rStats.highestCombo / (float)spawnedEnemies.Count));
                    print(score);
                    GameObject.Find("EndScene").GetComponent<Canvas>().enabled = true;
                    done = true;
                }
            }
        }
    }

    void StartMusic(){
        _audioSource.Play();
    }

    void GenerateNotes(){
        notes = new List<Vector2>();
        otherNoteInfo = new List<Vector2>();
        string fs = map.text;
        string[] maplines = fs.Split('\n');
        var metadata = maplines[0].Split(' ');
        print(metadata[0] + " " + metadata[1] + " " + metadata[2]);
        songBpm = float.Parse(metadata[0].Substring(1));
        
        print("songBpm:" + songBpm);
        endMarker = float.Parse(metadata[2]);
        //print(endMarker);
        float offset = float.Parse(metadata[1]);
        print("songOffset:" + offset);
        for(int i=1; i<maplines.Length; i++){
            string[] nn = maplines[i].Split(' ');
            if (nn[0].Contains("/") || nn.Length==1)
                continue;
            float lane = float.Parse(nn[0]); //0 = w; 1 = a; 2 = s; 3 = d
            float ty = float.Parse(nn[3]) + offset;
            //print(tx + " " + ty);
            notes.Add(new Vector2(lane, ty));
            
            //additional info
            int type = 0; // 0 = small, 1 = shield, -2 = large
            int color = 0; // 0 = color a, 1 = color b
            if (nn[1] == "SMALL") type = 0;
            else if (nn[1] == "SHIELD") type = 1;
            else if (nn[1] == "LARGE") type = -2;

            if (nn[2] == "COLORA") color = 0;
            else if (nn[2] == "COLORB") color = 1;

            otherNoteInfo.Add(new Vector2(type, color));
        }
    }

    public void HitKey(char track){
        currentTrack = track;
        //basic animation test
        /* int canim = Random.Range(0,2);
        while(lastanim == canim){
            canim = Random.Range(0,2);
        }
        lastanim = canim;
        playerAnims.PlayAnim(lastanim); */

        for(int i=0; i<6; i++){
            if((spawnedEnemiesInd+i) >= (spawnedEnemies.Count)) continue;
            if(spawnedEnemies[spawnedEnemiesInd+i] == null) continue;
            if(spawnedEnemies[spawnedEnemiesInd+i].isHit) continue;

            if(spawnedEnemies[spawnedEnemiesInd+i].track == track){
                //print("right track: " + track.ToString());
                //print("diff: " + Mathf.Abs(spawnedEnemies[spawnedEnemiesInd+i].beat - songPosInBeats).ToString());

                if (Mathf.Abs(spawnedEnemies[spawnedEnemiesInd + i].beat - songPosInBeats) <= enemyKillWindow){
                    //spawnedEnemies[spawnedEnemiesInd + i].Hit();
                    hitAccuracy = spawnedEnemies[spawnedEnemiesInd + i].beat - songPosInBeats;
                    combo += 1;
                    if (combo % 100 == 0 && combo!=0) {
                        //anim.Play();
                    }
                    if (combo > rStats.highestCombo)
                        rStats.highestCombo = combo;
                    //print("hitaccuracy: " + hitAccuracy.ToString());
                    switch (hitAccuracy) {
                        case float f when f< 0.13f:
                            //Perfect case
                            score += (int)((1 / 0.13f) * (scoreAmnt));
                            rStats.hitCounts[0] += 1;
                            break;
                        case float f when f< 0.2f:
                            //Great case
                            score += (int)((1 / 0.2f) * (scoreAmnt));
                            rStats.hitCounts[1] += 1;
                            break;
                        case float f when f < 0.3f:
                            //Ok case
                            score += (int)((1 / 0.3f) * (scoreAmnt));
                            rStats.hitCounts[2] += 1;
                            break;
                        case float f when f < 0.4f:
                            //Bad case
                            score += (int)((1 / 0.4f) * (scoreAmnt));
                            rStats.hitCounts[3] += 1;
                            break;
                    }
                    //UnityEngine.Debug.LogFormat("{0} {1} {2} {3} {4}", rStats.hitCounts[0], rStats.hitCounts[1], rStats.hitCounts[2], rStats.hitCounts[3], rStats.hitCounts[4]);
                    rStats.score = score;
                    //camshake.AddShake();
                    hitAudio.Play();
                    //spawnedEnemies[spawnedEnemiesInd + i].Hit();
                    hitAccuracy = spawnedEnemies[spawnedEnemiesInd + i].beat - songPosInBeats;

                    /* if(spawnedEnemies[spawnedEnemiesInd + i].noteType == NoteTypes.hold){
                        heldTracks[track] = true;
                        if(holdGhost != null){
                            float py = track == 0 ? laneY1 : laneY2;
                            HoldGhost tgh = Instantiate(holdGhost, new Vector3(playerGO.transform.position.x, py, 0), Quaternion.identity);
                            tgh.track = track;
                            tgh.endBeat = spawnedEnemies[spawnedEnemiesInd + i].endBeat;
                        }
                        heldNotes[track] = spawnedEnemies[spawnedEnemiesInd + i];
                        heldNotes[track].isHeld = true;
                        heldNotes[track].rootVisual.SetActive(false);
                    } */

                    break;
                }
            }
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;

        // pause and unpause game+music
        if (enabled) {
                AudioListener.pause = false;
            }
            else {
                AudioListener.pause = true;
            }
    }

/*     public void pauseAudio() {
        _audioSource.Pause();
    } */
}