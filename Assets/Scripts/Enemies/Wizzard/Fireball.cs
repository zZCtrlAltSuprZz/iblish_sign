using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float fireBallSpeed = 5f; // Velocidad del proyectil de fuego
    private WizzardIA wizzard;


    private void Start()
    {
        // Asignar velocidad inicial al proyectil
       // GetComponent<Rigidbody2D>().velocity = Vector2.right * fireBallSpeed;
        wizzard = GameObject.FindObjectOfType<WizzardIA>();
    }
    private void Update()
    {
        Destroy(gameObject, 6);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Colisionó con el jugador, destruir el proyectil
            PlayerData.currentVida -= wizzard.damage;
            Destroy(gameObject);
        }
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);

        }

    }
}
