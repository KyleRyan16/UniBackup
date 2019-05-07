using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDestroyer : MonoBehaviour {

    Collider2D ObjectDestroyCollider;
    Collider2D[] OverlapResult;
    ContactFilter2D contactFilter;
	// Use this for initialization
	void Start () {

        ObjectDestroyCollider = GetComponent<Collider2D>();

	}
	
	// Update is called once per frame
	void Update () {

        ObjectDestroyCollider.OverlapCollider(contactFilter, OverlapResult)


        if(OverlapResult.Length > 0)
        {
             Destroy(OverlapResult.GetValue(0));
        }
	}
}
