using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitGame : MonoBehaviour
{
    public int buttonWidth;
    public int buttonHeight;

    private Scene currentScene;

    private SceneHistory hist;

    private int origin_x;
    private int origin_y;
    // Start is called before the first frame update
    void Start()
    {
        hist = GameObject.Find("SceneHistory").GetComponent<SceneHistory>();
        buttonWidth = 200;
        buttonHeight = 50;
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Lose"){
            origin_x = (Screen.width - (Screen.width / 5)) - buttonWidth / 2;
        }
        else {
            origin_x = (Screen.width - (Screen.width / 4)) - buttonWidth / 2;
        }
        origin_y = Screen.height / 2 - buttonHeight * 2;
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (currentScene.name != "Lose"){
            if(GUI.Button(new Rect(origin_x, origin_y, buttonWidth, buttonHeight), "Main Menu")){
                hist.LoadScene("StartMenu");
            }

            if(GUI.Button(new Rect(origin_x, origin_y + buttonHeight + 20, buttonWidth, buttonHeight), "Quit")){
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
        }
        else {
            if(GUI.Button(new Rect(origin_x, origin_y, buttonWidth, buttonHeight), "Retry Level")){
                hist.PreviousScene();
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
}
