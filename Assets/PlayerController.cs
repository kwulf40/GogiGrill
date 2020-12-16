using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform itemSlot;

    public GameObject[] tableList = new GameObject[16];

    public UnityEngine.Events.UnityEvent customerOrderGet;

    public UnityEngine.Events.UnityEvent goodLeave;

    private Item heldItem = null;

    private int tableNumber = 0;

    public float speed = 20.0f;
    public float gravity = 9.8f;
    public float range = 2;
    
    public CharacterController controller;
    private Vector3 direction = Vector3.zero;
    private Collider newObject;

    public Item MenuItem;
	
	private AudioSource pickUp1;
	private AudioSource pickUp2;
	private AudioSource playerDroppingItem;
	private AudioSource seatingSound;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        tableList = GameObject.FindGameObjectsWithTag("Table");
        tableNumber = UnityEngine.Random.Range(0, 16);
    
		//for sounds
		pickUp1 = GetComponents<AudioSource>()[0];
		pickUp2 = GetComponents<AudioSource>()[1];
		playerDroppingItem = GetComponents<AudioSource>()[2];
		seatingSound = GetComponents<AudioSource>()[3];
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, range);

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
            charMove();
        }

        if (Input.GetButtonDown("Fire1")){ //Pickup/drop function
            Debug.Log("press");
            itemCheck(objects);
            customerSeat(objects);
            customerOrder(objects);
            customerCheck(objects);
        }

        if (Input.GetKeyDown(KeyCode.L)){
            SceneManager.LoadScene("Lose");
        }
        
        if (Input.GetKeyDown(KeyCode.K)){
            SceneManager.LoadScene("Win");
        }

        if (Input.GetKeyDown(KeyCode.P)){
            goodLeave.Invoke();
        }

    }

    void charMove(){
        if (controller.isGrounded){
            direction.x = Input.GetAxis("Horizontal");
            direction.z = Input.GetAxis("Vertical");
            direction = direction.normalized;
        }
        else{
            direction.y -= gravity * Time.deltaTime;
        }
        
        controller.Move(direction * speed *  Time.deltaTime);
        //CHARACTER MOVEMENT SOUND GOES HERE (OPTIONAL)
        
        if (Quaternion.LookRotation(direction).y != 0 || direction.z != 0){//rotates player to the direction they last moved in
           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 4.0f);
        }
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);//Prevents the player from rotating while moving
    }

    void itemCheck(Collider[] objects){
        if (heldItem){
                Drop(heldItem);
                Debug.Log("Drop");
            }
        else{
            foreach(Collider newObject in objects){
                if (newObject.tag == "Food" || newObject.tag == "Menu"){
                    if(newObject.gameObject.GetComponent<Item>()){
                        Item newItem = newObject.GetComponent<Item>();
                        Pickup(newItem);
                        Debug.Log("Pickup");
                    }
                }
            }
        }
    }

    void customerSeat(Collider[] objects){
        foreach(Collider newObject in objects){
            if (newObject.tag == "Customer"){
                if(newObject.gameObject.GetComponent<Customer>().isSeated == false){
                    seatCust(newObject.gameObject.GetComponent<Customer>());
                    Debug.Log("Seat");
                }
            }
        }
    }

    void customerOrder(Collider[] objects){
        foreach(Collider newObject in objects){
            if (newObject.tag == "Customer"){
                if(newObject.gameObject.GetComponent<Customer>()){
                    Customer checkCust = newObject.gameObject.GetComponent<Customer>();
                    if(checkCust.order == true){
                        customerOrderGet.Invoke();
                        Debug.Log("Order");
                        if (heldItem){
                            heldItem.tableNum = checkCust.tableNum;
                        }
                        checkCust.order = false;
                        checkCust.readyToEat = true;
                    }
                }
            }
        }
    }

    void customerCheck(Collider[] objects){
        foreach(Collider newObject in objects){
            if (newObject.tag == "Customer"){
                if(newObject.gameObject.GetComponent<Customer>()){
                    Customer checkCust = newObject.gameObject.GetComponent<Customer>();
                    AudioSource happyAudio = newObject.gameObject.GetComponents<AudioSource>()[1];
                    if (checkCust.checkReady == true && happyAudio.isPlaying == false){
                        Debug.Log("goodExit");
                        checkCust.isLeaving = true;
                        happyAudio.Play();
                        Destroy(newObject.gameObject, happyAudio.clip.length);
                        goodLeave.Invoke();
                    }
                }
            }
        }
    }

    public void generateOrder(){
        Item menu = (Item) Instantiate(MenuItem, itemSlot.position, itemSlot.rotation);
        menu.gameObject.transform.Rotate(new Vector3(90,180,0));
        Pickup(menu);
        Debug.Log("Menu");
        //PLAYER PICKUP SOUND GOES HERE
		pickUp1.Play();
    }

    void Pickup(Item item) {
        heldItem = item;

        item.isHeld = true;
        item.Rb.isKinematic = true;
        item.Rb.velocity = Vector3.zero;
        item.Rb.angularVelocity = Vector3.zero;

        item.transform.SetParent(itemSlot);

        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;

        if (item.gameObject.tag == "Menu"){
            item.transform.Rotate(new Vector3(90,0,0)); 
        }
        

        //PLAYER PICKUP SOUND GOES HERE
		pickUp2.Play();
    }

    void Drop(Item item){
        heldItem = null;

        item.isHeld = false;
        item.transform.SetParent(null);
        item.Rb.isKinematic = false;

        item.Rb.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);
        //PLAYER DROPPING ITEM SOUND GOES HERE
		playerDroppingItem.Play();
    }

    public void seatCust(Customer cust){
        while (cust.transform.parent == null || cust.transform.parent.name == "CustomerSpawner"){
            GameObject chair = tableList[tableNumber].transform.GetChild(0).GetChild(0).gameObject;
            if (chair.transform.childCount == 0){
                cust.transform.SetParent(chair.transform);
                cust.transform.position = new Vector3 (chair.transform.position.x, chair.transform.position.y + 2, chair.transform.position.z);

                if (cust.transform.childCount > 1){
                    for (int j = 1; j < cust.transform.childCount; j++){
                        GameObject groupChair = tableList[tableNumber].transform.GetChild((j)).GetChild(0).gameObject;
                        GameObject group = cust.transform.GetChild(j).gameObject;

                        group.transform.position = new Vector3(groupChair.transform.position.x, groupChair.transform.position.y + 2, groupChair.transform.position.z);
                    }
                }
            }
            else{
                tableNumber = UnityEngine.Random.Range(0, 15);
            } 
        }
        cust.setTableNum((tableNumber + 1));
        cust.menu = true; 
        cust.isSeated = true;
        //PLAYER SEATING CUSTOMER SOUND GOES HERE (OPTIONAL)
		seatingSound.Play();
    }
}

