using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public int badLeaveCount = 0, goodLeaveCount = 0, customerCount = 0;
    public bool spawningCheck = false;
    public float waitTime = 5.0f;
    public UnityEngine.Events.UnityEvent callSpawn;

    private int maxLostCount = 3, maxHappyCount = 5;
    private SceneHistory hist;
    private Text goodCount;
    private Text maxGoodCount;
    private Text badCount;
    private Text maxBadCount;

	
    // Start is called before the first frame update
    void Start()
    {
        hist = GameObject.Find("SceneHistory").GetComponent<SceneHistory>();
        badLeaveCount = 0;
        goodLeaveCount = 0;
        customerCount = 0;
        waitTime = UnityEngine.Random.Range(4, 7);
        callSpawn.Invoke();
        customerCount++;

        goodCount = transform.Find("Canvas").Find("background1").Find("happyCount").GetComponent<Text>();
        badCount = transform.Find("Canvas").Find("background2").Find("lostCount").GetComponent<Text>();

        maxGoodCount = transform.Find("Canvas").Find("background1").Find("maxHappyCount").GetComponent<Text>();
        maxBadCount = transform.Find("Canvas").Find("background2").Find("maxLostCount").GetComponent<Text>();

	}

    void Awake()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        goodCount.text = goodLeaveCount.ToString();
        badCount.text = badLeaveCount.ToString();
        maxGoodCount.text = maxHappyCount.ToString();
        maxBadCount.text = maxLostCount.ToString();

        if(badLeaveCount == maxLostCount){
            Debug.Log("End Game, lose.");
            hist.LoadScene("Lose");
        }
        if(goodLeaveCount == maxHappyCount){
            Debug.Log("Good Job!");
            hist.LoadScene("LevelComplete");
        }
        if (spawningCheck == false && customerCount < 4){
            spawnCheck();
        }
    }
    
    public void badLeave(){
        GameLogic eventSys = (GameLogic) GameObject.Find("EventSystem").GetComponent<GameLogic>();
        eventSys.badLeaveCount++;
        eventSys.customerCount--;
        Debug.Log("badLeave");
    }

    public void goodLeave(){
        GameLogic eventSys = (GameLogic) GameObject.Find("EventSystem").GetComponent<GameLogic>();
        eventSys.goodLeaveCount++;
        eventSys.customerCount--;
        Debug.Log("goodLeave");
    }

    public void spawnCheck(){
        GameLogic eventSys = (GameLogic) GameObject.Find("EventSystem").GetComponent<GameLogic>();
        spawningCheck = true;
        eventSys.StartCoroutine (eventSys.spawning());
        
    }

    public IEnumerator spawning(){
        GameObject spawner = GameObject.Find("CustomerSpawner");
        if (spawner.transform.childCount == 0){
            if(customerCount < 4){

                yield return new WaitForSeconds(waitTime);
                waitTime = UnityEngine.Random.Range(4, 7);
                callSpawn.Invoke();
                customerCount++;
                spawningCheck = false;
                yield return null;
            }
        }
    }
}
