using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SucklerAttackLogic : MonoBehaviour
{
    public bool CanAttack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanAttack = true;
            // El jugador está dentro del rango, detener el movimiento del AIPath.
            //StopPathfinding();
            //Attack();
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanAttack = false;
            //timeBtwAttack = startTimeBtwAttack;
            // El jugador salió del rango, reanudar el movimiento del AIPath.
            //ResumePathfinding();
        }
    }

}
