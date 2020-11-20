﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject newCustomer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(){
        var newCust = (GameObject) Instantiate(newCustomer, transform.position, transform.rotation);
        newCust.transform.Rotate(new Vector3(0,180,0));
        newCust.transform.SetParent(transform);
    }
}