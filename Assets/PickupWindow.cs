﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWindow : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent goodLeave;
    
    public UnityEngine.Events.UnityEvent badLeave;
    private float maxWaitTime = 20.0f;

    private float currentWaitTime = 20.0f; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.childCount > 0){
            currentWaitTime = currentWaitTime - Time.deltaTime;
        }
        if (currentWaitTime <= 0.0f && this.transform.childCount > 0){
            currentWaitTime = maxWaitTime;
            //bad leave audio here
            badLeave.Invoke();
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "ToGoOrder"){
            if (col.gameObject.GetComponent<Item>().tableNum == 0451 ){
                if (this.transform.childCount > 0){
                Destroy(col.gameObject);
                //good leave audio here
                Destroy(this.transform.GetChild(0).gameObject);
                goodLeave.Invoke();
                }
            }
            else{
                return;
            }
            
        }
    }
}