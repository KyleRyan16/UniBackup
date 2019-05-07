using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDestroyer : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Destroyed" + collision.gameObject.name);

        Destroy(collision.gameObject);
    }

}
