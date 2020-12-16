using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    private Rigidbody rb;
    
    public Rigidbody Rb => rb;

    public Transform target;

    public bool isHeld = false;
    public int tableNum;
    public float range = 1;
    public Camera mainCam;

    private Text tableText;
    private Canvas itemCanv;
    void Awake(){
        rb = GetComponent<Rigidbody>();
        tableText = transform.Find("Canvas").Find("Number").GetComponent<Text>();
        itemCanv = transform.Find("Canvas").GetComponent<Canvas>();
    }

    void Update(){
        Vector3 look = mainCam.transform.position - transform.position;
        Collider[] objects = Physics.OverlapSphere(transform.position, range);

        look.x = look.z = 0.0f;
        itemCanv.transform.LookAt(mainCam.transform.position - look);
        itemCanv.transform.Rotate(40,180,0);
        tableText.text = tableNum.ToString();
        wallCheck(objects);
    }

    void wallCheck(Collider[] objects){
        foreach(Collider newObject in objects){
            if (newObject.tag == "Wall" && isHeld == false){
                target = GameObject.Find("Player").transform;
                transform.LookAt(target, Vector3.up);
		        transform.position += transform.forward;
            }
        }
    }

    void setTableNum(int newTableNum){
        tableNum = newTableNum;
    }
}
