using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public Customer[] newCustomer = new Customer[3];
    public GameObject[] custGroup = new GameObject[3];
    public Critic fuzzy;
    public PlayerController player;

    private GameObject spawnParent;

    private int numOfCust = 1;
    private int custType = 0;

    private bool criticSpawn = false;
	
	private AudioSource CustomerArrive;
    // Start is called before the first frame update
    void Start()
    {
        spawnParent = GameObject.Find("CustomerSpawner");
    }

    void Awake()
    {
        CustomerArrive = GetComponents<AudioSource>()[0];
        spawnParent = GameObject.Find("CustomerSpawner");
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level3" && criticSpawn == false){
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            SpawnCritic(player);
            criticSpawn = true;
        } 
    }

    public void Spawn(){
        if (spawnParent == null){
            spawnParent = GameObject.Find("CustomerSpawner");
        }
        CustomerArrive = spawnParent.GetComponents<AudioSource>()[0];
        custType = Random.Range(0, 2);
        numOfCust = Random.Range(0, 4);

        GameObject newCust = Instantiate(newCustomer[custType], transform.position, transform.rotation).gameObject;
        newCust.transform.SetParent(spawnParent.transform);
        newCust.transform.position = spawnParent.transform.position;
        Customer newCustScript = newCust.GetComponent<Customer>();
        
        if (numOfCust > 0){
            for (int i = 0; i < numOfCust; i++){
                int groupRand = Random.Range(0, 3);
                var newCustGroup = (GameObject) Instantiate(custGroup[groupRand], (transform.position + new Vector3((2+(2*i)),0,0)), transform.rotation);
                newCustGroup.transform.position = (spawnParent.transform.position + new Vector3((2+(2*i)),0,0));
                newCustGroup.transform.SetParent(newCust.transform);
            }
            newCustScript.setTimers(numOfCust);
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

    public void SpawnCritic(PlayerController player){
        Critic newFuzzy = Instantiate(fuzzy, transform.position, transform.rotation);
        player.seatCritic(newFuzzy);
    }
}
