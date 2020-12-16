using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
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
       origin_x = Screen.width / 2 - buttonWidth / 2;
       origin_y = (Screen.height - (Screen.height / 6)) - buttonHeight * 2;
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(origin_x, origin_y, buttonWidth, buttonHeight), "Start Game")) {
            hist.LoadScene("Level1");
        }
        if(GUI.Button(new Rect(origin_x, origin_y + buttonHeight + 20, buttonWidth, buttonHeight), "Exit")) {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else 
                Application.Quit();
            #endif
        }
    }
}
