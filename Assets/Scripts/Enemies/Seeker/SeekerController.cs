using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeekerController : MonoBehaviour
{
    // Stats
    public float damage=2;
    public float moveSpeed=2;
    public float health = 6;

    public bool boss;



    Animator anim;
    AudioSource audioSource;

    //Patrol system
    [SerializeField] Transform[] patrolPoints; // puntos limite de patrulla
    public Vector2 leftLimit,rightLimit; // limite del patrol
    public int patrolDestination; // dice a donde debe dirigirse el enemigo 
    private float _patrolPointDistanceMax = 0.2f;
    DetectionArea area;


    //Chase system
    public bool isChasing;
    public float chaseDistance;
    public float distance;

    //Attack 
    public float attackDistance;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask playerMask;
    public float attackRange;
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    //referencia al objeto del player
    GameObject playerObject;
    PlayerController playerController;

    //Recibir daño color de sprites
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.white;
    private Color originalColor;

    public float hitDuration = 0.1f;
    public int numberOfHits = 3; // Número de veces que parpadeará al ser golpeado


    private void Start()
    {
        damage = 2;
        moveSpeed = 2;
        health = 6;
        //asigna stats en base al nivel del jugador
        SeteoDeStats();

        originalColor = spriteRenderer.color;
        //inicializo las posiciones de los dos limites
        leftLimit = patrolPoints[0].position;
        rightLimit = patrolPoints[1].position;

        area= GetComponent<DetectionArea>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();


        //script del jugador 
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
    }
    private void Update()
    {
        //Cogemos la referencia del jugador en el juego
        playerObject = GameObject.FindWithTag("Player");

        //distancia hasta el player
        distance = Vector2.Distance(transform.position, playerObject.transform.position);
        
        //si estas dentro del area
        if (area.detected)
        {
            ChasePlayer();
            
        }
        //si no hace patrol
        else
        {
            isChasing = false;
            PatrolDestinationAndMove();
            anim.SetBool("Attack", false);
        }

    }

    void SeteoDeStats()
    {
        if (boss)
        {
            damage = 10;
            health = 25;
            moveSpeed = 2.8f;
        }
        else
        {
            
            if (PlayerData.lvl > 5 && PlayerData.lvl <= 10)
            {
                damage = damage * 2;
                health = health * 1.5f;
                moveSpeed = moveSpeed + 0.25f;
            }
            else if (PlayerData.lvl > 10)
            {
                damage = damage * 3;
                health = health * 1.75f;
                moveSpeed = moveSpeed + 0.35f;
            }
            else if (PlayerData.lvl > 15)
            {
                damage = damage * 4;
                health = health * 2;
                moveSpeed = moveSpeed + 0.45f;
            }
            else if (PlayerData.lvl > 20)
            {
                damage = damage * 4.5f;
                health = health * 2.25f;
                moveSpeed = moveSpeed + 0.55f;
            }
            else if (PlayerData.lvl >= 25)
            {
                damage = damage * 5;
                health = health * 2.5f;
                moveSpeed = moveSpeed + 0.65f;
            }
        }

        
    }

    private void ChasePlayer()
    {
        //direccion hacia la que debe ir 
        Vector3 direccionAlJugador = (playerObject.transform.position - transform.position).normalized;
        direccionAlJugador.y = 0;  // Ignorar cambios en la vertical


        if (distance< attackDistance)
        {
            Invoke("Attack", 0.3f);

        }
        else
        {
            isChasing = true;
            if (playerObject.transform.position.x - transform.position.x >0)
            {
                RotateToPlayer();
                anim.SetBool("Attack", false);

                // Mover al enemigo solo en la dirección horizontal
                transform.Translate(direccionAlJugador * moveSpeed * Time.deltaTime, Space.World);
            }
            else
            {

                RotateToPlayer();
                anim.SetBool("Attack", false);

                transform.Translate(direccionAlJugador * moveSpeed * Time.deltaTime, Space.World);


            }


        }
        
        
        
       
        

    }

    public void DealDamageAttack()
    {
        Collider2D[] playerToDmg = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerMask);
        for (int i = 0; i < playerToDmg.Length; i++)
        {
            playerController.RecieveDamage(damage);
        }
    }

    private void Attack()
    {
        //si se puede atacar
        if (timeBtwAttack <= 0)
        {
            anim.SetBool("Attack", true);

            //reset timer
            timeBtwAttack = startTimeBtwAttack;

        }
        else 
        { 
            if (timeBtwAttack < 1.3)
            {
                anim.SetBool("Attack", false);
            }
            //reduce el cd
            timeBtwAttack -= Time.deltaTime;
            
        }



    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    void PatrolDestinationAndMove()
    {


        //dependiendo de cual sea el destino se dirige a el
        if (patrolDestination == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, leftLimit, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, leftLimit) < _patrolPointDistanceMax)
            {
                patrolDestination = 1;
                RotateToPPoint(0);




            }
        }
        if (patrolDestination == 1)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, rightLimit, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, rightLimit) < _patrolPointDistanceMax)
            {
                patrolDestination = 0;
                RotateToPPoint(1);

            }
        }
    }

    void InvertirRotacion()
    {
        // Obtener la rotación actual
        Quaternion rotacionActual = transform.rotation;

        // Calcular la rotación opuesta (180 grados)
        Quaternion rotacionInvertida = Quaternion.Euler(0, 180, 0) * rotacionActual;

        // Establecer la nueva rotación
        transform.rotation = rotacionInvertida;
    }

    void RotateToPlayer()
    {
       
        float direccionX = playerObject.transform.position.x - transform.position.x;

        // Cambiar la rotación para que el enemigo mire a la izquierda o derecha
        if (direccionX > 0)
        {
            // El jugador está a la derecha del enemigo
            transform.rotation = Quaternion.Euler(0, 0, 0); // Rotación para mirar hacia la derecha
        }
        else
        {
            // El jugador está a la izquierda del enemigo
            transform.rotation = Quaternion.Euler(0, 180, 0); // Rotación para mirar hacia la izquierda
        }
        
    }

    void RotateToPPoint(int patrol)
    {


        // Cambiar la rotación para que el enemigo mire a la izquierda o derecha
        if (patrol==1)
        {
            // El jugador está a la derecha del punto
            transform.rotation = Quaternion.Euler(0, 0, 0); // Rotación para mirar hacia la derecha
        }
        else if(patrol==0)
        {

            // El jugador está a la izquierda del punto
            transform.rotation = Quaternion.Euler(0, 180, 0); // Rotación para mirar hacia la izquierda
        }

    }

    public void ReceiveDamage(float dmg)
    {
        //cambio de color del sprite
        TakeHit();


        //realizacion de daño
        health-= dmg;

        // suma de xp o eventos al dañar y o muerte del enemigo
        if (health <= 0)
        {
            PlayerData.xp += 50;
            if (PlayerData.currentVida < PlayerData.totalVida)
            {
                PlayerData.currentVida+=PlayerData.currentLifeSteal;
            }

            anim.SetTrigger("Die");
            audioSource.Play();
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.enabled = false;
            Destroy(gameObject,1);
            PlayerData.contBase++;

            if (boss)
            {
                PlayerData.contKeys++;
                PlayerData.xp += 50;

            }

        }
    }

    public void TakeHit()
    {
        StartCoroutine(FlashingColor());
    }

    IEnumerator FlashingColor()
    {
        for (int i = 0; i < numberOfHits; i++)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(hitDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(hitDuration);
        }
    }

}
