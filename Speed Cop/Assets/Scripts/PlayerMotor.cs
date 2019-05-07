using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    Rigidbody2D rb;
    Vector2 targetPos;


    public float Yincriment;
    public float speed;

    // Use this for initialization
    void Start () {

        rb = GetComponent<Rigidbody2D>();
        // Initialise to be the same as current position, awaiting new value from input
        targetPos = transform.position;

	}
	
	// Update is called once per frame
	void Update () {

        // maxDistanceDelta in MoveTowards() becomes speed because it limits the rate at which input can incriment position towards targetpos
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W))
        {
            targetPos = new Vector2(transform.position.x, transform.position.y + Yincriment);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            targetPos = new Vector2(transform.position.x, transform.position.y - Yincriment);
        }



    }
}
