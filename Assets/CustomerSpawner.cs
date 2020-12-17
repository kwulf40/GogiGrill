using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Customer newCustomer;
    public GameObject custGroup;

    private int numOfCust = 1;
	
	private AudioSource CustomerArrive;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        CustomerArrive = GetComponents<AudioSource>()[0];
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(){
        numOfCust = Random.Range(0,3);

        Customer newCust = Instantiate(newCustomer, transform.position, transform.rotation);
        newCust.transform.Rotate(new Vector3(0,180,0));
        newCust.transform.SetParent(transform);
        
        if (numOfCust > 0){
            for (int i = 0; i < numOfCust; i++){
                var newCustGroup = (GameObject) Instantiate(custGroup, (transform.position + new Vector3((2+(2*i)),0,0)), transform.rotation);
                newCustGroup.transform.Rotate(new Vector3(0,180,0));
                newCustGroup.transform.SetParent(newCust.transform);
            }
            
            newCust.setTimers(numOfCust);
        }
		CustomerArrive.Play();
    }
    public void SpawnMobile(){
        GameObject newMobCust = Instantiate(custGroup, transform.position, transform.rotation);
        GameObject PickupWindow = GameObject.Find("RightWindowBottom").gameObject;
        newMobCust.transform.SetParent(PickupWindow.transform);
        newMobCust.transform.position = new Vector3(PickupWindow.transform.position.x, PickupWindow.transform.position.y + 4, PickupWindow.transform.position.z + 1);
        newMobCust.transform.Rotate(new Vector3(0,180,0));
        CustomerArrive.Play();
    }
}
