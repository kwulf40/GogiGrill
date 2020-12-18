using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Customer[] newCustomer = new Customer[3];
    public GameObject[] custGroup = new GameObject[3];

    private int numOfCust = 1;
    private int custType = 0;
	
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
        custType = Random.Range(0, 3);
        numOfCust = Random.Range(0, 4);

        Customer newCust = Instantiate(newCustomer[custType], transform.position, transform.rotation);
        newCust.transform.SetParent(transform);
        
        if (numOfCust > 0){
            for (int i = 0; i < numOfCust; i++){
                int groupRand = Random.Range(0, 3);
                var newCustGroup = (GameObject) Instantiate(custGroup[groupRand], (transform.position + new Vector3((2+(2*i)),0,0)), transform.rotation);
                newCustGroup.transform.SetParent(newCust.transform);
            }
            
            newCust.setTimers(numOfCust);
        }
		CustomerArrive.Play();
    }
    public void SpawnMobile(){
        int groupRand = Random.Range(0, 2);
        GameObject newMobCust = Instantiate(custGroup[groupRand], transform.position, transform.rotation);
        GameObject PickupWindow = GameObject.Find("RightWindowBottom").gameObject;
        newMobCust.transform.SetParent(PickupWindow.transform);
        newMobCust.transform.position = new Vector3(PickupWindow.transform.position.x, PickupWindow.transform.position.y, PickupWindow.transform.position.z + 3);
        CustomerArrive.Play();
    }
}
