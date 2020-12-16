using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.Events.UnityEvent badLeave;
    [SerializeField]
    public UnityEngine.Events.UnityEvent seated;

    public bool menu = false, eating = false, order = false, isSeated = false, readyToEat = false, checkReady = false;
    public bool spawnToggle = false;

    public bool isLeaving = false;
    private float maxLeaveTime = 50.0f;
    private float maxMenuTime = 60.0f;
    private float maxEatTime = 60.0f;
    private float leaveTimer = 5.0f;
    private float menuTimer = 1.0f;
    private float eatTimer = 10.0f;

    public int tableNum;
    bool destroyed = false;
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
	
	private AudioSource CustomerReadytoOrder;

    private AudioSource CustomerHappy;
	private AudioSource CustomerUnhappy;

    // Start is called before the first frame update
    void Awake()
    {
        leaveTimer = maxLeaveTime;
        eatTimer = UnityEngine.Random.Range(10, 20);
        maxEatTime = eatTimer;
        menuTimer = UnityEngine.Random.Range(5, 10);
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
		CustomerReadytoOrder = GetComponents<AudioSource>()[0];
        CustomerHappy = GetComponents<AudioSource>()[1];
		CustomerUnhappy  = GetComponents<AudioSource>()[2];
    }

    // Update is called once per frame
    void Update()
    {
        
        leaveTimerText.text = Math.Round(leaveTimer).ToString();
		leaveTimerBar.fillAmount = leaveTimer / maxLeaveTime;

        if (menu == false && eating == false && destroyed == false){
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
                //CUSTOMER READY TO ORDER SOUND
				CustomerReadytoOrder.Play();
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


            if (eatTimer <= 0){
                eating = false;
                checkReady = true;
                custFinished.gameObject.SetActive(true);
                eatToggle();
                leaveToggle();
            }
        }

        if(isSeated == true && spawnToggle == false){
            seated.Invoke();
            spawnToggle = true;
        }

        if (leaveTimer <= 0){
            if (destroyed == false && isLeaving == false){
                Debug.Log("exit");
                CustomerUnhappy.Play();
                custLeaving.gameObject.SetActive(true);
                badLeave.Invoke();
                Destroy(gameObject, CustomerUnhappy.clip.length);   
                destroyed = true;
            }
        }

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

    public void setTimers(int groupNum){
        maxLeaveTime = 50.0f + (5.0f * groupNum);

        leaveTimer = maxLeaveTime;
        eatTimer = UnityEngine.Random.Range((10 * groupNum), (15 * groupNum));
        maxEatTime = eatTimer;
        menuTimer = UnityEngine.Random.Range((5 * groupNum), (10 * groupNum));
        maxMenuTime = menuTimer;
    }

    public void setTableNum(int newTableNum){
        tableNum = newTableNum;
    }
}
