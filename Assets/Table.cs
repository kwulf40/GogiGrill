using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
	private AudioSource recieveFood;

    private Image criticNotif;
	
    // Start is called before the first frame update
    void Start()
    {
        recieveFood = GetComponents<AudioSource>()[0];
        criticNotif = transform.Find("CriticCanv").GetChild(0).GetComponent<Image>();
        criticNotif.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.Find("Chair 1").Find("Seat").childCount > 0){
            if (transform.Find("Chair 1").Find("Seat").GetChild(0).name == "Fuzzypaws(Clone)" && criticNotif.gameObject.activeSelf == false){
                criticNotif.gameObject.SetActive(true);
            }
        }
    }

    void OnCollisionEnter(Collision col){
        Customer temp;
        Critic temp2;
        if(col.gameObject.tag == "Food"){
            Item checkFood = col.gameObject.GetComponent<Item>();
            if(transform.Find("Chair 1").Find("Seat").childCount > 0){
                if (transform.Find("Chair 1").Find("Seat").GetChild(0).name != "Fuzzypaws(Clone)"){
                    temp = (Customer) transform.Find("Chair 1").Find("Seat").GetChild(0).GetComponent<Customer>();
                    if (temp.readyToEat == true && checkFood.tableNum == temp.tableNum){
                        Destroy(col.gameObject);
                        temp.readyToEat = false;
                        temp.eating = true;
                        temp.increaseLeave(10.0f);
                        //CUSTOMER RECIEVING FOOD SOUND GOES HERE
                        recieveFood.Play();
                    }
                }
                else{
                    temp2 = (Critic) transform.Find("Chair 1").Find("Seat").GetChild(0).GetComponent<Critic>();
                    if (temp2.readyToEat == true && checkFood.tableNum == temp2.tableNum){
                        Destroy(col.gameObject);
                        temp2.readyToEat = false;
                        temp2.eating = true;
                        temp2.increaseLeave(10.0f);
                        //CUSTOMER RECIEVING FOOD SOUND GOES HERE
                        recieveFood.Play();
                    }
                }
            }
        }
    }
}
