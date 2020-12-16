using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    public int buttonWidth;
    public int buttonHeight;
    private SceneHistory hist;

    private int origin_x;
    private int origin_y;
    // Start is called before the first frame update
    void Start()
    {
        hist = GameObject.Find("SceneHistory").GetComponent<SceneHistory>();
        buttonWidth = 200;
        buttonHeight = 50;
        origin_x = (Screen.width - (Screen.width / 5)) - buttonWidth / 2;
        origin_y = Screen.height / 2 - buttonHeight * 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnGUI()
    {

        if(GUI.Button(new Rect(origin_x, origin_y, buttonWidth, buttonHeight), "Next Level")){
            if (hist.GetPrevious() == "Level1"){
                Debug.Log(hist.GetPrevious());
                hist.LoadScene("Level2");
            }
            else if (hist.GetPrevious() == "Level2"){
                hist.LoadScene("Level3");
            }
            else {
                Debug.Log(hist.GetPrevious());
                hist.LoadScene("Win");
            }
        }

        if(GUI.Button(new Rect(origin_x, origin_y + buttonHeight + 20, buttonWidth, buttonHeight), "Main Menu")){
            hist.LoadScene("StartMenu");
        }

        if(GUI.Button(new Rect(origin_x, origin_y + (2 * buttonHeight) + 40, buttonWidth, buttonHeight), "Quit")){
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
