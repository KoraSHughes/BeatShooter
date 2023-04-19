using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public float songBpm;
    public float scPerBeat;
    public float songPosition;
    public float songPositionInBeats;
    public float dspSongTine;
    public float firstBeatOffSet;
    public AudioSource _audioSource;

    void Start() {
        GameObbject _audioSource = GetComponent<AudioSource>();
        secPerBeat = 60f  songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        
        _audioSource.Play();
    }

    
    void Update() {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffSet);
        songPositionInBeats = songPosition / secPerBeat;
    }
}
