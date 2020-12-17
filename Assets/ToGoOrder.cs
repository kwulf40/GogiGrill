using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToGoOrder : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.Events.UnityEvent badLeave;
    private float maxRingTime = 10.0f;
    private float currentRingTime = 10.0f;
    private float maxCustArriveTime = 10.0f;
    private float currentCustArriveTime = 10.0f;

    public bool ringing = false;

    public bool customerArriving = false;
    public Material idleMat;
    public Material ringMat;

    private GameObject tablet;
    // Start is called before the first frame update
    void Start()
    {
        tablet = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (ringing == true){
            tablet.GetComponent<MeshRenderer>().material = ringMat;
            currentRingTime = currentRingTime - Time.deltaTime;
        }
        if (currentRingTime <= 0.0f){
            tablet.GetComponent<MeshRenderer>().material = idleMat;
            ringing = false;
            currentRingTime = maxRingTime;
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
    }

    public void ring(){
        ringing = true;
    }

    public void pickup(){
        ringing = false;
        customerArriving = true;
        currentRingTime = maxRingTime;
        tablet.GetComponent<MeshRenderer>().material = idleMat;   
    }
}
