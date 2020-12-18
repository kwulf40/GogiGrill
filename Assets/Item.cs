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
    public float range = 0.001f;
    public Camera mainCam;
    private Vector3 defaultScale;
    private Text tableText;
    private Canvas itemCanv;
    void Awake(){
        defaultScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        tableText = transform.Find("Canvas").Find("Number").GetComponent<Text>();
        itemCanv = transform.Find("Canvas").GetComponent<Canvas>();
    }

    void Update(){
        if (transform.parent == null){
        if (transform.localScale.x != defaultScale.x || transform.localScale.y > defaultScale.y || transform.localScale.z > defaultScale.z ){
            transform.localScale = defaultScale;
        }
        }
        Vector3 look = mainCam.transform.position - transform.position;
        Collider[] objects = Physics.OverlapBox(transform.position, new Vector3(range,range,range));

        look.x = look.z = 0.0f;
        itemCanv.transform.LookAt(mainCam.transform.position - look);
        itemCanv.transform.Rotate(40,180,0);
        if (tableNum == 0451){
            tableText.text = "Mobile";
        }
        else{
            tableText.text = tableNum.ToString();
        }
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
