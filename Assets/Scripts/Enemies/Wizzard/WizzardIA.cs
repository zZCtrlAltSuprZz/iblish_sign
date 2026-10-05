using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizzardIA : MonoBehaviour
{
    // Stats
    public float moveSpeed = 2f; // Velocidad de patrulla
    public float damage = 3.5f;
    public float health = 5;
    public bool boss = false;


    // Animator 
    Animator anim;

    AudioSource audioSource;

    // Patrol
    public float patrolDistance = 2f; // Distancia que el enemigo patrulla en cada dirección
    private Vector3 initialPosition;  //limites en los que 
    private float leftLimit;        // se movera cuando patrulle
    private float rightLimit;
    private bool movingRight = true;

    // Fireball
    private AreaDetect attackTrigger;
    [SerializeField] Transform fireBallSpawn;
    [SerializeField] GameObject fireBallPrefab;
    public float fireBallSpeed = 5f;
    public float cooldown = 1f;
    public int contFireball=0;

    //Recibir daño color de sprites
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.black;
    public float hitDuration = 0.1f;
    public int numberOfHits = 3; // Número de veces que parpadeará al ser golpeado

    private Color originalColor;


    private GameObject player;


    void Start()
    {
        SeteoDeStats();

        player = GameObject.FindWithTag("Player");
        attackTrigger = GetComponentInChildren<AreaDetect>();
        anim=GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        originalColor = spriteRenderer.color;


        // se da valor a la posicion inicial y a los limites
        initialPosition = transform.position;
        leftLimit = initialPosition.x - patrolDistance;
        rightLimit = initialPosition.x + patrolDistance;
    }

    void Update()
    {
        // cooldown de ataque
        cooldown -= Time.deltaTime;

        // si el jugador entra en el rango de ataque
        if (attackTrigger.playerInRange)
        {
            // se rota en su direccion 
            RotateTowardsPlayer();
            if (cooldown <= 0)
            {
                // lanza proyectil de fuego
                FireProjectile();
            }
        }
        else if(cooldown<0)
        {
            // patrulla 
            Patrol();
        }

        // Si el jugador se va del area se resetea el contador de bolas de fuego 
        if (!attackTrigger.playerInRange)
        {
            contFireball = 0;
        }
       

    }

    void SeteoDeStats()
    {
        if (boss)
        {
            damage = 15;
            health = 20;
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

    // Funcion que rota al enemigo para que mire derecha o izq dependiendo de donde este el player situado 
    private void RotateTowardsPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = transform.position.y;


        float dotProduct = Vector3.Dot(transform.forward, direction);
        if (dotProduct < 0)
        {
            transform.Rotate(0,180,0);
        }

    }

    void FireProjectile()
    {
        contFireball++;
        if (contFireball >= 4)
        {
            cooldown = 5f;
            contFireball = 0;
        }
        else
        {
            cooldown = 1f;
        }

        
        // Calcular la dirección hacia el jugador
        Vector2 direction = (player.transform.position - fireBallSpawn.position).normalized;

        // Instanciar la fireball en el punto de origen y orientarla hacia el jugador
        anim.SetTrigger("Attack");
        GameObject fireball = Instantiate(fireBallPrefab, fireBallSpawn.position, Quaternion.identity);
        fireball.transform.right = direction;

        // Obtener el componente Rigidbody2D del proyectil y aplicar velocidad en la dirección calculada
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireBallSpeed; // Aquí debes definir la velocidad de la fireball

        
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
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.enabled = false;
            Destroy(gameObject, 1f);
            PlayerData.contWizzard++;

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
