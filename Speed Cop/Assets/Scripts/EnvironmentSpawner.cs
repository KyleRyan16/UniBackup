using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour {


    // Parameters
    private Quaternion spawnRotation;
    public static float spawnOffset;
    public static bool waitToSpawnObject;

    // References
    public GameObject environment_1;
    public GameObject environment_2;
    public GameObject environment_3;
    public GameObject environment_4;

    public GameObject spawnPoint;
    public GameObject environmentMover;
    GameObject newPlatform;
    

    private void Awake()
    {
        spawnOffset = 120f;
        spawnRotation = new Quaternion(0, 0, 0, 0);

    }

    public void GenerateEnvironment(Vector3 destroyedObjectPosition)
    {

        destroyedObjectPosition.x += spawnOffset;
        GameObject prevPlatform = newPlatform;
        GameObject chosenPlatformInstantiate;

        if (!prevPlatform)
        {
            prevPlatform = GameObject.FindGameObjectWithTag("EnvironmentMover").transform.GetChild(4).gameObject;
        }


        float randomResult = Random.Range(0, 4);

        if (randomResult < 1)
        {
            chosenPlatformInstantiate = environment_1;
        }
        else if (randomResult < 2)
        {
            chosenPlatformInstantiate = environment_2;
        }
        else if (randomResult < 3)
        {
            chosenPlatformInstantiate = environment_3;
        }
        else chosenPlatformInstantiate = environment_4;


        //prevPlatform.transform.position;
        float posXOffset = (chosenPlatformInstantiate.transform.GetChild(1).lossyScale.x / 2) + (prevPlatform.transform.GetChild(1).lossyScale.x / 2);

        Vector3 spawnPosition = new Vector3(prevPlatform.transform.position.x + posXOffset, 0, 0);

        newPlatform = Instantiate(
            environment_1, 
            spawnPosition, 
            spawnRotation, 
            environmentMover.transform
            );

        EnvironmentMotor.buildings.Add(newPlatform);

        waitToSpawnObject = false;

    }
}
