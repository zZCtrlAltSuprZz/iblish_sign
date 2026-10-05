using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScorpionIA : MonoBehaviour
{
    // Stats
    public float damage = 3f;
    public float moveSpeed = 2f; // Velocidad de patrulla
    public float health = 5;
    public bool boss = false;

    Animator anim;
    AudioSource audioSource;

    // Patrol
    public float patrolDistance = 5f; // Distancia que el enemigo patrulla en cada dirección
    public float attackDistance = 0.5f;

    private Vector3 initialPosition;  //limites en los que 
    private float leftLimit;        // se movera cuando patrulle
    private float rightLimit;
    private bool movingRight = true;

    public float cooldown = 3f;

  
    // Elementos hijos y player
    private AreaDetect attackTrigger;
    private AttackDetect attackDetect;
    private WallDetector wallDetector;
    private GameObject player;

    //Recibir daño color de sprites
    public SpriteRenderer spriteRenderer;
    public Color hitColor;
    public float hitDuration = 0.1f;
    public int numberOfHits = 3; // Número de veces que parpadeará al ser golpeado

    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        //Stats
        SeteoDeStats();

        player = GameObject.FindWithTag("Player");
        attackTrigger = GetComponentInChildren<AreaDetect>();
        attackDetect = GetComponentInChildren<AttackDetect>();
        wallDetector=GetComponentInChildren<WallDetector>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        originalColor = spriteRenderer.color;



        // se da valor a la posicion inicial y a los limites
        initialPosition = transform.position;
        leftLimit = initialPosition.x - patrolDistance;
        rightLimit = initialPosition.x + patrolDistance;
    }

    // Update is called once per frame
    void Update()
    {
        // cooldown de ataque
        cooldown -= Time.deltaTime;

        if (!attackTrigger.playerInRange)
        {
            Patrol();
        }
        else if(attackTrigger.playerInRange && attackDetect.touchingGround && !wallDetector.isTouchingWall)
        {
            // Calcular la dirección hacia el jugador
            float distancia = (player.transform.position.x - transform.position.x);
            if (distancia < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            // Si el jugador está a una distancia mayor que la distancia para atacar, moverse hacia él
            if (Mathf.Abs(distancia) > attackDistance)
            {
                // Mover el escorpión en el eje X
                transform.Translate(Vector2.right * moveSpeed * 2f * Time.deltaTime);
            }
            // Si el jugador está dentro de la distancia para atacar, detenerse y atacar
            else if(cooldown<=0)
            {
                anim.SetTrigger("Attack");
                if (attackDetect.playerInRange)
                {
                    PlayerData.currentVida -= damage;
                }
                cooldown = 3f;
            }
        }
    }

    void SeteoDeStats()
    {
        if (boss)
        {
            damage = 8;
            health = 20;
            moveSpeed = 4;
        }
        else
        {

            if (PlayerData.lvl > 5 && PlayerData.lvl <= 10)
            {
                damage = damage * 1.5f;
                health = health * 1.5f;
                moveSpeed = moveSpeed + 0.25f;
            }
            else if (PlayerData.lvl > 10)
            {
                damage = damage * 2;
                health = health * 1.75f;
                moveSpeed = moveSpeed + 0.35f;
            }
            else if (PlayerData.lvl > 15)
            {
                damage = damage * 2.5f;
                health = health * 2;
                moveSpeed = moveSpeed + 0.45f;
            }
            else if (PlayerData.lvl > 20)
            {
                damage = damage * 3;
                health = health * 2.25f;
                moveSpeed = moveSpeed + 0.55f;
            }
            else if (PlayerData.lvl >= 25)
            {
                damage = damage * 3.25f;
                health = health * 2.5f;
                moveSpeed = moveSpeed + 0.65f;
            }
        }
    }


    void Patrol()
    {

        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);


        // Si alcanza el límite derecho, cambia de dirección
        if (transform.position.x >= rightLimit)
        {
            movingRight = false;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        // Si alcanza el límite izquierdo, cambia de dirección
        if (transform.position.x <= leftLimit)
        {
            movingRight = true;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void ReceiveDamage(float dmg)
    {
        //cambio de color del sprite
        TakeHit();


        //realizacion de daño
        health -= dmg;

        // suma de xp o eventos al dañar y o muerte del enemigo
        if (health <= 0)
        {
            PlayerData.xp += 75;
            //muerte
            if (PlayerData.currentVida < PlayerData.totalVida)
            {
                PlayerData.currentVida += PlayerData.currentLifeSteal;
            }
            anim.SetTrigger("Die");
            audioSource.Play();
            CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
            capsuleCollider.enabled = false;
            Destroy(gameObject, 1);
            PlayerData.contScorpion++;
            
            if (boss)
            {
                PlayerData.xp += 75;
                PlayerData.contKeys++;
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
