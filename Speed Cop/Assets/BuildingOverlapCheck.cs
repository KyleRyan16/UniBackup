using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingOverlapCheck : MonoBehaviour {

    // References
    EnvironmentDestroyer environmentDestroyer;
    GameManager GameManager;
    // Use this for initialization
    private void Awake()
    {
        environmentDestroyer = FindObjectOfType<EnvironmentDestroyer>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Building")
        {
            environmentDestroyer.DestroyBuilding(collision);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
