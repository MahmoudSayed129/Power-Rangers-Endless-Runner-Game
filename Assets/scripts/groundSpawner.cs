using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundSpawner : MonoBehaviour
{
    public GameObject ground;
    Vector3 nextSpawnPoint;
    // Start is called before the first frame update

    public void Spawn(bool generate)
    {
        GameObject newGround = Instantiate(ground, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = newGround.transform.GetChild(1).transform.position;
        if (generate)
        {
            newGround.GetComponent<groundTile>().spawnObstacle();
        }
    }
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            if(i < 3)
            {
                Spawn(false);
            }
            else
            {
                Spawn(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
