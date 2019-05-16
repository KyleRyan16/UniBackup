using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDestroyer : MonoBehaviour {


    // Parameters
    Transform destroyedEnvironmentTransform;
    Vector3 destroyedObjectPosition;
    ContactFilter2D contactFilter;

    // References
    Collider2D DestroyArea;
    Collider2D[] overlapResults;
    EnvironmentSpawner environmentSpawner;

    private void Awake()
    {
        environmentSpawner = transform.GetComponentInChildren<EnvironmentSpawner>();

        DestroyArea = GameObject.FindGameObjectWithTag("DestroyArea").transform.GetComponent<Collider2D>();

        contactFilter.layerMask = LayerMask.GetMask("Building");

    }

    private void Update()
    {

        

    }

    

    public void DestroyBuilding(Collider2D other)
    {

        EnvironmentMotor.buildings.Remove(other.transform.gameObject);

        destroyedEnvironmentTransform = other.transform;
        destroyedObjectPosition = new Vector3(destroyedEnvironmentTransform.position.x, destroyedEnvironmentTransform.position.y, 0);


        EnvironmentSpawner.waitToSpawnObject = true;

        environmentSpawner.GenerateEnvironment(other.transform.position);

        Destroy(other.gameObject);
    }

}
