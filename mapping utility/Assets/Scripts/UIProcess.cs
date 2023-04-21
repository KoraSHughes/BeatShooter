using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIProcess : MonoBehaviour
{
    public GameObject manager;
    public GameObject browserObj;
    GameObject inputselect;
    Browser browser;
    AudioImporter importer;
    public AudioClip song;
    public void Start(){
        GameObject.Find("RecordingCanvas").GetComponent<Canvas>().enabled = false;
        importer = GetComponent<NAudioImporter>();    
    }
    public void GetSongPath() {
        inputselect =GameObject.Instantiate(browserObj, GameObject.Find("InputCanvas").transform);
        browser = inputselect.GetComponent<Browser>();
        browser.FileSelected += OnFileSelected;
        importer.Loaded += OnLoaded;
        inputselect.transform.position = new Vector3(inputselect.transform.position.x, inputselect.transform.position.y - 50, inputselect.transform.position.z);
    }
    public void WakeUpManager() {
        //simple little guy!
        GameObject.Instantiate(manager);
        GameObject.Find("RecordingCanvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("SavedText").GetComponent<TextMeshProUGUI>().enabled = false;
    }

    private void OnFileSelected(string path) {
        importer.Import(path);
    }

    private void OnLoaded(AudioClip clip){
        song = clip;
        //print(song);
        inputselect.SetActive(false);
    }
}
