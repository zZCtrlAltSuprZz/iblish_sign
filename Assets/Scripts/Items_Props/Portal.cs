using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject enemigoPrefab;
    [SerializeField] string tipoBoss;

    bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            if (tipoBoss == "seeker" && PlayerData.contBase >= 15)
            {
                SpawnearBoss();
                spawned = true;
            }
            else if (tipoBoss == "suckler" && PlayerData.contSuckler >= 15)
            {
                SpawnearBoss();
                spawned = true;
            }
            else if (tipoBoss == "scorpion" && PlayerData.contScorpion >= 15)
            {
                SpawnearBoss();
                spawned = true;
            }
            else if (tipoBoss == "wizzard" && PlayerData.contWizzard >= 15)
            {
                SpawnearBoss();
                spawned = true;
            }
        }
    }
    void SpawnearBoss()
    {
        if(tipoBoss== "seeker")
        {
            Instantiate(enemigoPrefab, spawnPoint.position, Quaternion.identity);
            enemigoPrefab.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y-1.4f, spawnPoint.transform.position.z);
        }
        else if (tipoBoss == "scorpion")
        {
            Instantiate(enemigoPrefab, spawnPoint.position, Quaternion.identity);
            enemigoPrefab.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y - 1.4f, spawnPoint.transform.position.z);
        }
        else if (tipoBoss == "wizzard")
        {
            Instantiate(enemigoPrefab, spawnPoint.position, Quaternion.identity);
            enemigoPrefab.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y - 1.4f, spawnPoint.transform.position.z);
        }
        else 
        {
            Instantiate(enemigoPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
