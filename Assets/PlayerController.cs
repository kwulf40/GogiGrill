using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

/*
* PlayerController.cs for Gogi Grill
* Made for CSUSM CS 485 Fall 2020 
* Audio variables and calls all done by Ronalyn Castilla
* All other code in this and all game files authored by Keegan Wulf
*
*
* KBBCrew:
* Ronalyn Castilla
* Neala Mendoza
* Keegan Wulf
* Rishi Ramrakhyani
*/
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform itemSlot;

    public GameObject[] tableList = new GameObject[16];

    public UnityEngine.Events.UnityEvent customerOrderGet;

    public UnityEngine.Events.UnityEvent goodLeave;

    private Item heldItem = null;

    private Image pauseMenu;

    private int tableNumber = 0;

    private bool pauseToggle = false;

    public float speed = 20.0f;
    public float gravity = 9.8f;
    public float range = 2;
    
    public CharacterController controller;
    private Vector3 direction = Vector3.zero;
    private Collider newObject;

    public Item MenuItem;

    //Scene Controller
    private SceneHistory hist;

    //GUI
    public int buttonWidth;
    public int buttonHeight;
    private int origin_x;
    private int origin_y;

	//Audio
	private AudioSource pickUp1;
	private AudioSource pickUp2;
	private AudioSource playerDroppingItem;
	private AudioSource seatingSound;
	private AudioSource phonePickup;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        pauseMenu = transform.Find("PauseMenu").Find("Image").GetComponent<Image>();
        pauseMenu.gameObject.SetActive(false);
        tableList = GameObject.FindGameObjectsWithTag("Table");
        for (int i = 1; i <= 16; i++){//for sorting table list
            string temp = "Table " + i.ToString();
            tableList[i - 1] = GameObject.Find(temp);
        }
        tableNumber = UnityEngine.Random.Range(0, 15);

        //Scene Controller
        hist = GameObject.Find("SceneHistory").GetComponent<SceneHistory>();

        //for GUI
        buttonWidth = 200;
        buttonHeight = 50;
        origin_x = (Screen.width / 2 - buttonWidth / 2) - 25;
        origin_y = Screen.height / 2 - buttonHeight * 2;
    
		//for sounds
		pickUp1 = GetComponents<AudioSource>()[0];
		pickUp2 = GetComponents<AudioSource>()[1];
		playerDroppingItem = GetComponents<AudioSource>()[2];
		seatingSound = GetComponents<AudioSource>()[3];
		phonePickup = GetComponents<AudioSource>()[4];
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
            pickupPhone(objects);
            criticOrder(objects);
            criticCheck(objects);
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
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (pauseToggle == true){
                pauseToggle = false;
            }
            else {
                pauseToggle = true;
            }
        }
        if (pauseToggle == true){
            Time.timeScale = 0;
            pauseMenu.gameObject.SetActive(true);
        }
        else {
            Time.timeScale = 1;
            pauseMenu.gameObject.SetActive(false);
        }

    }

    void OnGUI(){
        if (pauseToggle == true){
            Debug.Log("Pause");
            if(GUI.Button(new Rect(origin_x, origin_y, buttonWidth, buttonHeight), "Resume")){
                pauseToggle = false;
            }


            if(GUI.Button(new Rect(origin_x, origin_y + buttonHeight + 20, buttonWidth, buttonHeight), "Main Menu")){
                hist.LoadScene("StartMenu");
            }

            if(GUI.Button(new Rect(origin_x, origin_y + (2 * buttonHeight) + 40, buttonWidth, buttonHeight), "Quit")){
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
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
           transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.LookRotation(direction * 2)), 4.0f);
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
                if (newObject.tag == "Food" || newObject.tag == "Menu" || newObject.tag == "ToGoOrder"){
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
            if (newObject.tag == "Customer" && newObject.name != "Fuzzypaws(Clone)"){
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
                            Debug.Log(checkCust.tableNum.ToString());
                        }
                        checkCust.order = false;
                        checkCust.readyToEat = true;
                    }
                }
            }
        }
    }

    void criticOrder(Collider[] objects){
        foreach(Collider newObject in objects){
            if (newObject.tag == "Customer" && newObject.name == "Fuzzypaws(Clone)"){
                if(newObject.gameObject.GetComponent<Critic>()){
                    Critic checkCust = newObject.gameObject.GetComponent<Critic>();
                    if(checkCust.order == true){
                        customerOrderGet.Invoke();
                        Debug.Log("Order");
                        if (heldItem){
                            heldItem.tableNum = checkCust.tableNum;
                            Debug.Log(checkCust.tableNum.ToString());
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

    void criticCheck(Collider[] objects){
        foreach(Collider newObject in objects){
            if (newObject.tag == "Customer" && newObject.name == "Fuzzypaws(Clone)"){
                if(newObject.gameObject.GetComponent<Critic>()){
                    Critic checkCust = newObject.gameObject.GetComponent<Critic>();
                    if (checkCust.checkReady == true){
                        Debug.Log("winner");
                        checkCust.isLeaving = true;
                    }
                }
            }
        }
    }

    void pickupPhone(Collider[] objects){
        foreach(Collider newObject in objects){
            if (newObject.tag == "ToGo"){
                if(newObject.gameObject.GetComponent<ToGoOrder>()){
                    ToGoOrder phone = newObject.gameObject.GetComponent<ToGoOrder>();
                    //Phone Pickup noise here -> AudioSource callAudio = newObject.gameObject.GetComponents<AudioSource>()[];
                    if (phone.ringing == true){
                        Debug.Log("Pickup Phone");
                        //Audio call here -> callAudio.Play();
						phonePickup.Play();
                        phone.pickup();
                        generateOrder();
                        Debug.Log("Generate mobile menu");
                        if(heldItem){
                            heldItem.tableNum = 0451;
                        }
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
                cust.transform.position = new Vector3 (chair.transform.position.x, chair.transform.position.y - 0.5f, chair.transform.position.z);
                cust.transform.GetChild(0).transform.Rotate(new Vector3 (0,-90,0));
                cust.increaseLeave(10.0f);

                if (cust.transform.childCount > 1){
                    int chairNum = 1;
                    for (int j = 2; j < cust.transform.childCount; j++){
                        
                        GameObject groupChair = tableList[tableNumber].transform.GetChild((chairNum)).GetChild(0).gameObject;
                        GameObject group = cust.transform.GetChild(j).gameObject;

                        group.transform.position = new Vector3(groupChair.transform.position.x, groupChair.transform.position.y - 0.5f, groupChair.transform.position.z);
                        chairNum++;
                        if (j % 2 == 0){
                            group.transform.Rotate(new Vector3 (0, 90,0));
                        }
                        else {
                            group.transform.Rotate(new Vector3 (0,-90,0));
                        }
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

    public void seatCritic(Critic cust){
        while (cust.transform.parent == null || cust.transform.parent.name == "CustomerSpawner"){
            GameObject chair = tableList[tableNumber].transform.GetChild(0).GetChild(0).gameObject;
            if (chair.transform.childCount == 0){
                cust.transform.SetParent(chair.transform);
                cust.transform.position = new Vector3 (chair.transform.position.x + 1.1f, chair.transform.position.y -0.44f, chair.transform.position.z);
                cust.transform.GetChild(0).transform.Rotate(new Vector3 (-40,-90,0));

            }
            else{
                tableNumber = UnityEngine.Random.Range(0, 15);
            } 
        }
        cust.setTableNum((tableNumber + 1));
        cust.menu = true; 
        cust.isSeated = true;
    }
}

