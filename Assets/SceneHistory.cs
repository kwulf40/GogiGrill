using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHistory : MonoBehaviour
{

    private List<string> sceneHistory = new List<string>();   
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string newScene){
        sceneHistory.Add(newScene);
        SceneManager.LoadScene(newScene);
    }

    public bool PreviousScene(){
        bool returnValue = false;
        if (sceneHistory.Count >= 2)
        {
            Debug.Log("Load Hist");
            returnValue = true;
            sceneHistory.RemoveAt(sceneHistory.Count -1);
            SceneManager.LoadScene(sceneHistory[sceneHistory.Count -1]);
        }
 
        return returnValue;
    }

    public string GetPrevious(){
        return (sceneHistory[sceneHistory.Count - 2]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
