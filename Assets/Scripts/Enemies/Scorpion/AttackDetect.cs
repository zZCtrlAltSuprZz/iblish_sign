using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackDetect : MonoBehaviour
{
    //true cuando esta dentro, false cuando esta fuera
    public bool playerInRange = false;
    public bool touchingGround = true;

    private void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
        if (collision.CompareTag("Ground"))
        {
            touchingGround = true;
        }
       

    }
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
        if (collision.CompareTag("Ground"))
        {
            touchingGround = false;
        }

    }
}
