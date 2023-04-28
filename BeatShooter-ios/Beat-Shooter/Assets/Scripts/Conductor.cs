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
    public List<Vector3> otherNoteInfo;
    public int nextIndex;
    public float beatsSpawned;
    public float songPlayOffset;

    public float hitAccuracy;

    public static Conductor instance;

    //public MusicNote notePrefab;
    //public MusicNote hazardPrefab;
    
    public GameObject top;
    public GameObject bottom;
    public GameObject left;
    public GameObject right;

    public GameObject enemy1;
    public GameObject enemy2;

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
        secPerBeat = 60f / songBpm;

        dspSongTime = (float)AudioSettings.dspTime;
        spawnedEnemies = new List<Enemy>();
        Invoke("StartMusic", songPlayOffset);
        GenerateNotes();
        nextIndex = 0;
        spawnedEnemiesInd = 0;
        score = 0;
        combo = 0;
        GameObject.Find("EndScene").GetComponent<Canvas>().enabled = false;
    }
    
    void Update() {
        songPos = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffSet);
        songPosInBeats = songPos / secPerBeat;

        // SPAAAAAAAAAAAAAWN NOTES
        if(nextIndex < notes.Count && notes[nextIndex].y < songPosInBeats + enemiesSpawned){
            //print(spawnpoint.position + " " + Quaternion.identity);
            Enemy _enemy;
            NoteTypes nt = NoteTypes.single;
            if(Mathf.RoundToInt(otherNoteInfo[nextIndex].x) == 0 || Mathf.RoundToInt(otherNoteInfo[nextIndex].x) == 1){ //Note is not a hazard
                _enemy = Instantiate(notePrefab, spawnpoint.position, Quaternion.identity);
                if(Mathf.RoundToInt(otherNoteInfo[nextIndex].x) == 1){
                    nt = NoteTypes.hold;
                }
            }else{ // note is a hazard
                _enemy = Instantiate(hazardPrefab, spawnpoint.position, Quaternion.identity);
                nt = NoteTypes.hazard;
            }

            float ty = notes[nextIndex].x == 0 ? laneY1 : laneY2;

            m.Initialize(ty, startX, endX, notes[nextIndex].y, notes[nextIndex].x, otherNoteInfo[nextIndex].y, nt);
            spawnedNotes.Add(m);
            nextIndex++;
        }
        
        for(int i = 0; i<2; i++){
            if(heldTracks[i]){
                if(heldNotes[i]){
                    holdParticles[i].SetActive(true);
                    if(holdTimer[i] <= 0){
                        holdTimer[i] = holdInterval;
                        combo++;
                        hitAudio.Play();
                    }
                }else{
                    holdParticles[i].SetActive(false);
                }
                
                if(holdTimer[i] > 0)
                    holdTimer[i] -= Time.deltaTime;
            }else{
                for(int pi=i*2; pi<(i*2+2); pi++){
                    holdParticles[i].SetActive(false);
                }
            }
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
        otherNoteInfo = new List<Vector3>();
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
            char lane = float.Parse(nn[0]);
            float ty = float.Parse(nn[2]) + offset;
            //print(tx + " " + ty);
            notes.Add(new Vector2(lane, ty));
            
            //additional info (TYPEVAL: 1 = color a, 2 = color b)
            int typeVal = 0;
            if (nn[1] == "COLORA") {
                typeVal = 1;
            }else if (nn[1] == "COLORB") {
                typeVal = 2;
            }

            otherNoteInfo.Add(new Vector3(typeVal, 0));
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