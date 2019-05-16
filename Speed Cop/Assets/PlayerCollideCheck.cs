using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollideCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "Player")
        {
            Destroy(other);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
    }
}
