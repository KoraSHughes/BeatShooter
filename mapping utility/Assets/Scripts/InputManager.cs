using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class InputManager : MonoBehaviour
{
    public KeyCode[] left, up, down, right;
    string direction; //l: 0, u: 1, d: 2, r: 3
    enum NoteType{COLORA, COLORB, SMALL, SHIELD, LARGE}; //doing a bit 
    //public List<Vector2> notes; //y is beat, x is position
    List<string> mapLst;
    int lastPos, escCount, type;
    float held0start, held1start, held2start, held3start;
    float holdCheck = 0.5f;

    private void pressDownHelper(int color) {
        //if (Input.GetKey(KeyCode.Space)) {
            print("Pressed!\n");
            mapLst.Add(direction + " " + (type == 0 ? NoteType.SMALL: type == 1 ? NoteType.SHIELD : NoteType.LARGE)
            + (color == 0 ? NoteType.COLORA : NoteType.COLORB) + " " + Conductor.songPosInBeats + "\n");
        //}
/*         else {
            if (held1start == -1) {
                held1start = Conductor.songPosInBeats;
            }
        } */
    }

/*     private void holdHelper(float heldstart) {
        if (Mathf.Abs(Conductor.songPosInBeats - heldstart) > holdCheck) {
            mapLst.Add(direction + " " + heldstart + " " + Conductor.songPosInBeats + "\n"); //+ NoteType.HELD
        }
        else {
            mapLst.Add(direction + " " + heldstart + "\n"); //+ NoteType.SINGLE
        }
    } */

    private void Start() {
        escCount = 0;
        lastPos = 0;
        type = 0;
        // held1start = -1;
        // held2start = -1;
        mapLst = new List<string>();
    }

    void Update() {
        // log the time for mapping analysis use
        int pos = (int)Conductor.songPos;
        if (pos != lastPos && pos % 10 == 0) {
            print("Here " + pos + " " + lastPos);
            lastPos = pos;
            mapLst.Add("/*" + Conductor.songPos + " current pos*/\n");
        }

        if (Input.GetKey(KeyCode.Space)) 
            type = 1;
        else if (Input.GetKey(KeyCode.LeftShift))
            type = 2;

        //left press down check
        if (Input.GetKeyDown(left[0])) {
            direction = "a";
            pressDownHelper(0);
        }
        else if (Input.GetKeyDown(left[1])) {
            direction = "a";
            pressDownHelper(1);
        }
        //up press down check
        else if (Input.GetKeyDown(up[0])) {
            direction = "w";
            pressDownHelper(0);
        }
        else if (Input.GetKeyDown(up[1])) {
            direction = "w";
            pressDownHelper(1);
        }

        else if (Input.GetKeyDown(down[0])) {
            direction = "s";
            pressDownHelper(0);
        }
        else if (Input.GetKeyDown(down[1])) {
            direction = "s";
            pressDownHelper(1);
        }

        else if (Input.GetKeyDown(right[0])) {
            direction = "d";
            pressDownHelper(0);
        }
        else if (Input.GetKeyDown(right[1])) {
            direction = "d";
            pressDownHelper(1);
        }
        
        
/*         //left release check
        else if (Input.GetKeyUp(left[0]) || Input.GetKeyUp(left[1])) {
            if (held0start != -1) {
                holdHelper(held0start);
                held0start = -1;
            }
        }
        //up release check
        else if (Input.GetKeyUp(up[0]) || Input.GetKeyUp(up[1])) {
            if (held1start != -1) {
                holdHelper(held1start);
                held1start = -1;
            }
        }
        else if (Input.GetKeyUp(up[0]) || Input.GetKeyUp(up[1])) {
            if (held2start != -1) {
                holdHelper(held2start);
                held2start = -1;
            }
        }
        else if (Input.GetKeyUp(up[0]) || Input.GetKeyUp(up[1])) {
            if (held3start != -1) {
                holdHelper(held3start);
                held3start = -1;
            }
        } */

        /*else if (Input.GetKeyUp(left[0]) || Input.GetKeyUp(left[1])) {
            if (Mathf.Abs(Conductor.songPosInBeats - held1start) > holdCheck)
            {
                //threshhold for held notes
                mapLst.Add("0 " + NoteType.HELD + " " + held1start + " " + Conductor.songPosInBeats + "\n");
            }
            else
            { //add a kraft singles to the map list
                mapLst.Add("0 " + NoteType.SINGLE + " " + held1start + "\n");
            }
            held1start = -1;
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            // add a quick hazard 
            mapLst.Add("0 " + NoteType.HAZARD + " " + Conductor.songPosInBeats + "\n");
        }


        //track2 input check
        if (Input.GetKeyDown(up[0]) || Input.GetKeyDown(up[1]))
        {
            if (held2start == -1)
            {
                held2start = Conductor.songPosInBeats;

            }
        }
        else if (Input.GetKeyUp(up[0]) || Input.GetKeyUp(up[1]))
        {
            if (Mathf.Abs(Conductor.songPosInBeats - held2start) > holdCheck)
            {
                //threshhold for held notes
                mapLst.Add("1 " + NoteType.HELD + " " + held2start + " " + Conductor.songPosInBeats + "\n");
            }
            else
            {
                mapLst.Add("1 " + NoteType.SINGLE + " " + held2start + "\n");
            }
            held2start = -1;
        }
        else if (Input.GetKeyDown(KeyCode.H)) {
            mapLst.Add("1 " + NoteType.HAZARD + " " + Conductor.songPosInBeats + "\n");

        }*/

        //misc keys (esc, anything else we need idk) 
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (escCount == 0)
            {
                var savedText = GameObject.Find("SavedText");
                savedText.GetComponent<TextMeshProUGUI>().enabled = true;
                GameObject c = GameObject.Find("Manager");
                if (c == null) {
                    savedText.GetComponent<TextMeshProUGUI>().text = "You pressed escape too early--" +
                        "nothing was generated. Press esc again to restart process\n";
                    escCount++;
                    return;
                }
                Conductor cond = c.GetComponent<Conductor>();
                string preInput = "{" +cond.songBpm + " " 
                    +cond.firstBeatOffset + " " 
                    +cond.musicSource.clip.length + " " 
                    +"\""+cond.musicSource.clip.name+"\""+" "
                    + "}\n";
                string mapStr = preInput + string.Join("", mapLst);
                Regex r = new Regex("[\\s\"\'*<>\\|\\/:?]|($|\\s\\.)"); //attempt to replace illegal characters lol
                string sanitizedSongName = r.Replace(cond.musicSource.clip.name, "_");
                //sanitizedSongName = sanitizedSongName.Replace(" ", "_");
                sanitizedSongName += "_map.txt";
                System.IO.File.WriteAllText(sanitizedSongName, mapStr);
                savedText.GetComponent<TextMeshProUGUI>().text += sanitizedSongName+"\nPress escape again to reset\n";
                cond.musicSource.Stop();
                escCount++;
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    /* deprecated idea of cleaning the map for double presses
     * before current scheme was reached
    string CleanMap(string map) {
        //TODO: clean the map!
        string cleanmap = map;
        return cleanmap;
    }*/

}
