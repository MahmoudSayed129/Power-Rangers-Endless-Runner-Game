using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundTile : MonoBehaviour
{

    groundSpawner groundSpawner;
    public GameObject obstacle;
    public GameObject redEnergy;
    public GameObject greenEnergy;
    public GameObject blueEnergy;
    // Start is called before the first frame update
    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<groundSpawner>();
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.Spawn(true);
        Destroy(gameObject, 2);
    }

    public void spawnObstacle()
    {
        // pick random number 2--4
        int energyExist = Random.Range(0, 2); // 0 -> noEnergy 1 -> energy exist
        int energyType = Random.Range(0, 4); // 0 -> red 1-> green 2 -> blue
        int numObstacles = Random.Range(1, 3); // 1 or 2 obstacles
        int obstaclesPosition1 = Random.Range(2, 5);
        int obstaclesPosition2 = Random.Range(2, 5);
        while (obstaclesPosition2 == obstaclesPosition1)
        {
            obstaclesPosition2 = Random.Range(2, 5);
        }
        int energyPostion = Random.Range(2, 5);
        while (energyPostion == obstaclesPosition1 || obstaclesPosition2 == energyPostion)
        {
            energyPostion = Random.Range(2, 5);
        }
        Transform point = transform.GetChild(obstaclesPosition1).transform;
        Instantiate(obstacle, point.position, Quaternion.identity, transform);
        if(numObstacles == 2)
        {
            Transform point2 = transform.GetChild(obstaclesPosition2).transform;
            Instantiate(obstacle, point2.position, Quaternion.identity, transform);
        }
        if(energyExist == 1)
        {
            Transform point3 = transform.GetChild(energyPostion).transform;
            Vector3 postion = point3.position;
            postion.y = postion.y + 0.5f;
            if (energyType == 0)
            {
                Instantiate(redEnergy, postion, Quaternion.identity, transform);
            } 
            else if (energyType == 1)
            {
                Instantiate(greenEnergy, postion, Quaternion.identity, transform);
            }
            else
            {
                Instantiate(blueEnergy, postion, Quaternion.identity, transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
