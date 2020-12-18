using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Critic : MonoBehaviour
{
    public bool menu = false, eating = false, order = false, isSeated = false, readyToEat = false, checkReady = false, delay = false;
    public bool isLeaving = false;

    private int timesEaten = 0;
    private float eatDelay = 10.0f;
    private float maxEatDelay = 10.0f;
    private float maxLeaveTime = 180.0f;
    private float maxMenuTime = 60.0f;
    private float maxEatTime = 60.0f;
    private float leaveTimer = 5.0f;
    private float menuTimer = 1.0f;
    private float eatTimer = 10.0f;

    public int tableNum;
    
    private SceneHistory hist;

    //GUI
    private Text leaveTimerText;
    private Image leaveTimerBar;
    private Image leaveTimerBack;

    private Text menuTimerText;
    private Image menuTimerBar;
    private Image menuTimerBack;

    private Text eatTimerText;
    private Image eatTimerBar;
    private Image eatTimerBack;

    private Image custLeaving;
    private Image custOrder;
    private Image custFinished;
	
    //Audio
	private AudioSource CustomerReadytoOrder;
    private AudioSource CustomerHappy;
	private AudioSource CustomerUnhappy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake(){
        hist = GameObject.Find("SceneHistory").GetComponent<SceneHistory>();

        leaveTimer = maxLeaveTime;//set leave timer to the value of max leave time
        eatTimer = UnityEngine.Random.Range(10, 30); // how long they will take to eat
        maxEatTime = eatTimer;
        menuTimer = UnityEngine.Random.Range(5, 20);
        maxMenuTime = menuTimer;

        leaveTimerText = transform.Find("Canvas").Find("Background").Find("timeBarText").GetComponent<Text>();
		leaveTimerBar = transform.Find("Canvas").Find("Background").Find("timeImage").Find("timeImage2").GetComponent<Image>();
        leaveTimerBack = transform.Find("Canvas").Find("Background").Find("timeImage").GetComponent<Image>();

        menuTimerText = transform.Find("Canvas").Find("Background").Find("menuBarText").GetComponent<Text>();
		menuTimerBar = transform.Find("Canvas").Find("Background").Find("menuImage").Find("menuImage2").GetComponent<Image>();
        menuTimerBack = transform.Find("Canvas").Find("Background").Find("menuImage").GetComponent<Image>();

        eatTimerText = transform.Find("Canvas").Find("Background").Find("eatBarText").GetComponent<Text>();
		eatTimerBar = transform.Find("Canvas").Find("Background").Find("eatImage").Find("eatImage2").GetComponent<Image>();
        eatTimerBack = transform.Find("Canvas").Find("Background").Find("eatImage").GetComponent<Image>();

        custFinished = transform.Find("Canvas").Find("Background").Find("CustFin").GetComponent<Image>();
        custLeaving = transform.Find("Canvas").Find("Background").Find("CustMad").GetComponent<Image>();
        custOrder = transform.Find("Canvas").Find("Background").Find("CustOrder").GetComponent<Image>();

        if (leaveTimerText.gameObject.activeSelf == false){
            leaveToggle();
        }
        menuTimerText.gameObject.SetActive(false);
        menuTimerBar.gameObject.SetActive(false);
        menuTimerBack.gameObject.SetActive(false);
        eatTimerText.gameObject.SetActive(false);
        eatTimerBar.gameObject.SetActive(false);
        eatTimerBack.gameObject.SetActive(false);
        custFinished.gameObject.SetActive(false);
        custOrder.gameObject.SetActive(false);
        custLeaving.gameObject.SetActive(false);
		
        //for Sounds
		//CustomerReadytoOrder = GetComponents<AudioSource>()[0];
        //CustomerHappy = GetComponents<AudioSource>()[1];
		//CustomerUnhappy  = GetComponents<AudioSource>()[2];
    }

    // Update is called once per frame
    void Update()
    {
        leaveTimerText.text = Math.Round(leaveTimer).ToString();
		leaveTimerBar.fillAmount = leaveTimer / maxLeaveTime;

        if (menu == false && eating == false){
            leaveTimer = leaveTimer - Time.deltaTime;
        }

        if (menu == true){
            if (leaveTimerText.gameObject.activeSelf == true){
                leaveToggle();
                menuToggle();
            }
            
            menuTimer = menuTimer - Time.deltaTime;

            menuTimerText.text = Math.Round(menuTimer).ToString();
		    menuTimerBar.fillAmount = menuTimer / maxMenuTime;

            if (menuTimer <= 0){
                menu = false;
                custOrder.gameObject.SetActive(true);
                order = true;
                menuTimer = maxMenuTime;
				//CustomerReadytoOrder.Play();
                menuToggle();
                leaveToggle();
                
            }
        }

        if (readyToEat == true && custOrder.gameObject.activeSelf == true){
            custOrder.gameObject.SetActive(false);
        }

        if (eating == true){

            if (leaveTimerText.gameObject.activeSelf == true){
                leaveToggle();
                eatToggle();
            }
            
            eatTimer = eatTimer - Time.deltaTime;

            eatTimerText.text = Math.Round(eatTimer).ToString();
		    eatTimerBar.fillAmount = eatTimer / maxEatTime;


            if (eatTimer <= 0 && timesEaten == 2){
                eating = false;
                checkReady = true;
                custFinished.gameObject.SetActive(true);
                eatToggle();
                leaveToggle();
                hist.LoadScene("Win");
            }
            else if (eatTimer <= 0){
                eating = false;
                delay = true;
                timesEaten++;
                eatToggle();
                leaveToggle();
            }
        }

        if (delay == true){
            eatDelay = eatDelay - Time.deltaTime;
        }
        if (eatDelay <= 0.0f){
            delay = false;
            eatDelay = maxEatDelay;
            menu = true;
        }

        if (leaveTimer <= 0){
            hist.LoadScene("Lose");
        }
    }

    public void setTableNum(int newTableNum){
        tableNum = newTableNum;
    }

    public void increaseLeave (float time){
        this.leaveTimer = this.leaveTimer + time;
    }

    private void leaveToggle(){
        if (leaveTimerText.gameObject.activeSelf == true){
            leaveTimerText.gameObject.SetActive(false);
            leaveTimerBar.gameObject.SetActive(false);
            leaveTimerBack.gameObject.SetActive(false);
        }
        else{
            leaveTimerText.gameObject.SetActive(true);
            leaveTimerBar.gameObject.SetActive(true);
            leaveTimerBack.gameObject.SetActive(true);
        }
    }

    private void menuToggle(){
        if (menuTimerText.gameObject.activeSelf == true){
            menuTimerText.gameObject.SetActive(false);
            menuTimerBar.gameObject.SetActive(false);
            menuTimerBack.gameObject.SetActive(false);
        }
        else{
            menuTimerText.gameObject.SetActive(true);
            menuTimerBar.gameObject.SetActive(true);
            menuTimerBack.gameObject.SetActive(true);
        }
    }

    private void eatToggle(){
        if (eatTimerText.gameObject.activeSelf == true){
            eatTimerText.gameObject.SetActive(false);
            eatTimerBar.gameObject.SetActive(false);
            eatTimerBack.gameObject.SetActive(false);
        }
        else{
            eatTimerText.gameObject.SetActive(true);
            eatTimerBar.gameObject.SetActive(true);
            eatTimerBack.gameObject.SetActive(true);
        }
    }
}

    