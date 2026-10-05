using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public Transform[] spawnerPosiciones;
    public float tiempoEntreSpawns;
    public int maximoEnemigos = 8; // Número máximo de enemigos activos.
    public GameObject[] enemigosActivos;

    public float detectionRadius=5;

    public string tagEnemy;

    AreaDetect areaDetect;

    public float tiempoParaSiguienteSpawn = 0f;

    private void Start()
    {
        areaDetect=GetComponentInChildren<AreaDetect>();
    }
    void Update()
    {
        tiempoParaSiguienteSpawn += Time.deltaTime;
        // Verificar si es tiempo de spawnear un enemigo y si no hemos alcanzado el máximo.
        if (tiempoParaSiguienteSpawn >= tiempoEntreSpawns && ContarEnemigosActivos() < maximoEnemigos && areaDetect.playerInRange)
        {
           
            
            SpawnearEnemigo();
            tiempoParaSiguienteSpawn = 0;
            
            
        }
    }

    private bool IsEnemyNearSpawn(int spawnIndex)
    {
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemigosActivos)
        {
            float distance = Vector3.Distance(enemy.transform.position, spawnerPosiciones[spawnIndex].position);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
            
        }

        return minDistance < detectionRadius;
    }
    void SpawnearEnemigo()
    {
        int indiceSpawner = Random.Range(0, spawnerPosiciones.Length);
        if (!IsEnemyNearSpawn(indiceSpawner))
        {
            Transform posicionSpawner = spawnerPosiciones[indiceSpawner];

            Instantiate(enemigoPrefab, posicionSpawner.position, Quaternion.identity);
        }
         
    }

    int ContarEnemigosActivos()
    {
        // Contar el número de enemigos activos en la escena.
        enemigosActivos = GameObject.FindGameObjectsWithTag(tagEnemy); // Ajusta la etiqueta según tu configuración.
        return enemigosActivos.Length;
    }
}
