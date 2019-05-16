using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMotor : MonoBehaviour {

    // Parameters
    private Vector2 positionPointTransform2D; // Tranform with removed Z axis because game is 2D, used for Vector2 methods.
    public static float distance; // Amount of distance per second environment moves.
    public static float maxDistance = 30;
    private float sumOfBuildingLengths;


    // References
    public static List<GameObject> buildings = new List<GameObject>();

    private void Awake()
    {
        do ManageBuildingReferences();
        while (buildings.Count < transform.childCount);

    }

    private void ManageBuildingReferences()
    {

        while (buildings.Count < transform.childCount)
        {
            buildings.Add(transform.GetChild(buildings.Count).gameObject);



        }

        sumOfBuildingLengths = 0;
        int x = 0;
        do
        {
            sumOfBuildingLengths += buildings[x].transform.GetChild(0).lossyScale.x;
            x += 1;

        } while (x < buildings.Count);

        EnvironmentSpawner.spawnOffset = sumOfBuildingLengths;
        
    }

    // Use this for initialization
    void Start () {

        
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (!EnvironmentSpawner.waitToSpawnObject)
        {
            if (distance >= maxDistance)
            {
                distance = maxDistance;
            }
            else distance += 0.02f;
        }

        ManageBuildingReferences();
        MoveBuildings();

    }

    private void MoveBuildings()
    {

        int x = 0;
        do
        {
            // Take 3D transform Vector and create 2D variant.
            Vector2 buildingTransform2D = new Vector2(
            buildings[x].transform.position.x,
                buildings[x].transform.position.y
                );

            // Move object towards referenced object with fixed point in scene
            buildings[x].transform.position = Vector2.MoveTowards(
                buildingTransform2D,
                buildingTransform2D - new Vector2(distance, 0),
                distance * Time.deltaTime
                );
            x += 1;
        }
        while (x < buildings.Count);
    }
}
