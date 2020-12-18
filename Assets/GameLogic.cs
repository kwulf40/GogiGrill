using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public int badLeaveCount = 0, goodLeaveCount = 0, customerCount = 0, maxCustomerCount = 4;
    public bool spawningCheck = false;
    public float waitTime = 5.0f;
    public UnityEngine.Events.UnityEvent callSpawn;

    private int maxLostCount = 3, maxHappyCount = 5;
    private SceneHistory hist;

    private ToGoOrder tablet;

    private float delay = 3.0f;

    private float maxCallTimer = 30.0f;
    private float currentCallTimer = 30.0f;
    private bool sendRing = false;
    private bool firstSpawn = false;
    private Scene currentScene;
    private Text goodCount;
    private Text maxGoodCount;
    private Text badCount;
    private Text maxBadCount;
    private Text level3Text;
    private Text happyText;
    private Text divider;
	
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        hist = GameObject.Find("SceneHistory").GetComponent<SceneHistory>();
        badLeaveCount = 0;
        goodLeaveCount = 0;
        customerCount = 0;
        waitTime = UnityEngine.Random.Range(4, 7);
        if (currentScene.name != "Level3"){
            callSpawn.Invoke();
        }
        customerCount++;

        goodCount = transform.Find("Canvas").Find("background1").Find("happyCount").GetComponent<Text>();
        badCount = transform.Find("Canvas").Find("background2").Find("lostCount").GetComponent<Text>();

        maxGoodCount = transform.Find("Canvas").Find("background1").Find("maxHappyCount").GetComponent<Text>();
        maxBadCount = transform.Find("Canvas").Find("background2").Find("maxLostCount").GetComponent<Text>();

        happyText = transform.Find("Canvas").Find("background1").Find("happyText").GetComponent<Text>();;
        divider = transform.Find("Canvas").Find("background1").Find("Divider").GetComponent<Text>();;

        level3Text = transform.Find("Canvas").Find("background1").Find("Level3Text").GetComponent<Text>();
        level3Text.gameObject.SetActive(false);

        if (currentScene.name == "Level2"){
            tablet = GameObject.Find("ToGoTablet").GetComponent<ToGoOrder>();

            maxLostCount = 4;
            maxHappyCount = 8;
            maxCustomerCount = 6;
        }
        else if (currentScene.name == "Level3"){
            tablet = GameObject.Find("ToGoTablet").GetComponent<ToGoOrder>();

            maxLostCount = 5;
            maxHappyCount = 100;
            maxCustomerCount = 6;

            happyText.gameObject.SetActive(false);
            divider.gameObject.SetActive(false);
            maxGoodCount.gameObject.SetActive(false);
            goodCount.gameObject.SetActive(false);
            level3Text.gameObject.SetActive(true);

        }

	}

    void Awake()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        delay = delay - Time.deltaTime;
        if (currentScene.name == "Level3" && delay <= 0.0f && firstSpawn == false){
            spawnCheck();
            firstSpawn = true;
        }
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
            if (currentScene.name != "Level3"){
                hist.LoadScene("LevelComplete");
            }
            else {
                hist.LoadScene("Win");
            }
        }
        if (spawningCheck == false && customerCount < maxCustomerCount){
            spawnCheck();
        }

        if (currentScene.name == "Level2" || currentScene.name == "Level3"){
            if (sendRing == false){
                currentCallTimer = currentCallTimer - Time.deltaTime;
            }
            if (currentCallTimer <= 0.0 && sendRing == false) {
                sendRing = true;
                tablet.ring();
                Debug.Log("Ring");
            }
            if (sendRing == true){
                maxCallTimer = Random.Range(30.0f, 50.0f);
                currentCallTimer = maxCallTimer;
                sendRing = false;
            }
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

    public void badMobLeave(){
        GameLogic eventSys = (GameLogic) GameObject.Find("EventSystem").GetComponent<GameLogic>();
        eventSys.badLeaveCount++;
        Debug.Log("badLeave");
    }

    public void goodMobLeave(){
        GameLogic eventSys = (GameLogic) GameObject.Find("EventSystem").GetComponent<GameLogic>();
        eventSys.goodLeaveCount++;
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
            if(customerCount < maxCustomerCount){

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
