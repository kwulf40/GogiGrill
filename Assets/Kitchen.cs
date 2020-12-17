using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kitchen : MonoBehaviour
{
    public bool cooking = false;
    public float cookTimer = 10.0f;

    public Item FoodItem;

    public Item mobileFoodItem;

    public int tableNum;

    private Text cookTime;

    private Text dispTable;
	
	private AudioSource foodReadySound;
	private AudioSource OrderRecievedSound;
	private AudioSource CookingSound;
    // Start is called before the first frame update
    void Start()
    {
        cookTime = transform.Find("FoodCounter").Find("Canvas").Find("FoodCount").GetComponent<Text>();
        dispTable = transform.Find("TableNum").Find("Canvas").Find("Table").GetComponent<Text>();
        cookTime.gameObject.SetActive(false);
        dispTable.gameObject.SetActive(false);
		
		//for sounds
		foodReadySound = GetComponents<AudioSource>()[0];
		OrderRecievedSound = GetComponents<AudioSource>()[1];
		CookingSound = GetComponents<AudioSource>()[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (cooking == true){
            cookTimer = cookTimer - Time.deltaTime;

            if(cookTime.gameObject.activeSelf == false){
                cookTime.gameObject.SetActive(true);
                dispTable.gameObject.SetActive(true);
            }

            cookTime.text = Math.Round(cookTimer).ToString();
            if (tableNum != 0451){
                dispTable.text = "Cooking table: " + tableNum.ToString();
            }
            else {
                dispTable.text = "Cooking Mobile";
            }
           
            if (cookTimer <= 0 && tableNum != 0451){
                cooking = false;
                cookTime.gameObject.SetActive(false);
                dispTable.gameObject.SetActive(false);
                Item newFood = (Item) Instantiate(FoodItem, transform.position, transform.rotation);
                newFood.gameObject.transform.position = new Vector3(25, 8, 1);
                newFood.tableNum = tableNum;
                cookTimer = 10.0f;
				foodReadySound.Play();
            }

            else if (cookTimer <= 0 && tableNum == 0451){
                cooking = false;
                cookTime.gameObject.SetActive(false);
                dispTable.gameObject.SetActive(false);
                Item mobileFood = (Item) Instantiate(mobileFoodItem, transform.position, transform.rotation);
                mobileFood.gameObject.transform.Rotate(new Vector3(90, 0, 0));
                mobileFood.gameObject.transform.position = new Vector3(25, 8, 5);
                mobileFood.tableNum = tableNum;
                cookTimer = 10.0f;
				foodReadySound.Play();
            }
            else {
                return;
            }
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Menu"){
            if (cooking == false){
                Item tempItem = col.gameObject.GetComponent<Item>();
                tableNum = tempItem.tableNum;
                Destroy(col.gameObject);
                cooking = true;
                //ORDER RECIEVED SOUND GOES HEREs
			    OrderRecievedSound.Play();
			    CookingSound.Play();
            }
            else{
                return;
            }
            
        }
    }
}
