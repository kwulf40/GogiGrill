using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToGoOrder : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.Events.UnityEvent badLeave;
    private float maxRingTime = 10.0f;
    private float currentRingTime = 10.0f;
    private float maxCustArriveTime = 10.0f;
    private float currentCustArriveTime = 10.0f;

    private bool loopTog = false;

    public bool ringing = false;

    public bool customerArriving = false;
    public Material idleMat;
    public Material ringMat;

    private GameObject tablet;
	
	private AudioSource phoneRing;
	private AudioSource customerUnhappy;

    private Image alert;
    
    // Start is called before the first frame update
    void Start()
    {
        tablet = this.gameObject;
		alert = transform.Find("Canvas").Find("PhoneAlert").GetComponent<Image>();
        alert.gameObject.SetActive(false);
		customerUnhappy = GetComponents<AudioSource>()[0];
		phoneRing = GetComponents<AudioSource>()[1];
    }

    void Awake()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (ringing == true){
            tablet.GetComponent<MeshRenderer>().material = ringMat;
            currentRingTime = currentRingTime - Time.deltaTime;
            alert.gameObject.SetActive(true);
            //Phone ring sound here for maxRingTime duration
            if (loopTog == false){
                phoneRing.Play();
			    loopTog = true;
            }
        }
        if (currentRingTime <= 0.0f){
            tablet.GetComponent<MeshRenderer>().material = idleMat;
            ringing = false;
            alert.gameObject.SetActive(false);
            currentRingTime = maxRingTime;
            loopTog = false;
            phoneRing.Stop();
            //bad leave sound
			customerUnhappy.Play();
            badLeave.Invoke();
        }

        if (GameObject.Find("RightWindowBottom").transform.childCount < 1 && customerArriving == true){
            currentCustArriveTime = currentCustArriveTime - Time.deltaTime;
            if (currentCustArriveTime <= 0.0f){
                GameObject.Find("CustomerSpawner").GetComponent<CustomerSpawner>().SpawnMobile();
                currentCustArriveTime = maxCustArriveTime;
                customerArriving = false;
            }
        }
        phoneRing.loop = loopTog;
    }

    public void ring(){
        ringing = true;
    }

    public void pickup(){
        ringing = false;
        alert.gameObject.SetActive(false);
        customerArriving = true;
        loopTog = false;
        currentRingTime = maxRingTime;
        tablet.GetComponent<MeshRenderer>().material = idleMat;   
    }
}
