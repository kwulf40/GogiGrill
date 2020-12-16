using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
	private AudioSource recieveFood;
	
    // Start is called before the first frame update
    void Start()
    {
        recieveFood = GetComponents<AudioSource>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col){
        Customer temp;
        if(col.gameObject.tag == "Food"){
            Item checkFood = col.gameObject.GetComponent<Item>();
            if(transform.Find("Chair 1").Find("Seat").childCount > 0){
                temp = (Customer) transform.Find("Chair 1").Find("Seat").GetChild(0).GetComponent<Customer>();
                if (temp.readyToEat == true && checkFood.tableNum == temp.tableNum){
                    Destroy(col.gameObject);
                    temp.readyToEat = false;
                    temp.eating = true;
                    //CUSTOMER RECIEVING FOOD SOUND GOES HERE
					recieveFood.Play();
                }
            }
        }
    }
}
