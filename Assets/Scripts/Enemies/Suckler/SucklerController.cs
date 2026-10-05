using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SucklerController : MonoBehaviour
{
    public float moveSpeed=2; 
    public float health=4;
    public float damage=2;
    public bool boss=false;

    private AIPath aiPath;
    Animator anim;
    AudioSource audioSource;


    public float timeBtwAttack;
    public float startTimeBtwAttack=0.5f;
    public float attackRange;


    PlayerController playerController;
    [SerializeField] LayerMask playerMask;
    [SerializeField] Transform attackPoint;
    public bool CanAttack;

    SucklerAttackLogic sucklerAttLog;

    //Feedback AL RECIBIR DAÑO
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float hitDuration = 0.1f;
    public int numberOfHits = 2; // Número de veces que parpadeará al ser golpeado

    private Color originalColor;

    void Start()
    {
        //stats
        SeteoDeStats();

        originalColor = spriteRenderer.color;
        timeBtwAttack = startTimeBtwAttack;
        // Obtén la referencia al componente AIPath.

        sucklerAttLog = GetComponentInChildren<SucklerAttackLogic>();
        aiPath = GetComponent<AIPath>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        aiPath.maxSpeed = moveSpeed;

    }

    private void Update()
    {
        CanAttack=sucklerAttLog.CanAttack;
        /*if(timeBtwAttack >=0 && timeBtwAttack <= startTimeBtwAttack)
        {
            timeBtwAttack -= Time.deltaTime;
        }*/

        if (CanAttack)
        {
            StopPathfinding();
            Attack();
        }
        else
        {
            timeBtwAttack = startTimeBtwAttack;
            // El jugador salió del rango, reanudar el movimiento del AIPath.
            ResumePathfinding();
        }

       
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanAttack = true;
            // El jugador está dentro del rango, detener el movimiento del AIPath.
            StopPathfinding();
            Attack();
        }
    }

   
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanAttack = false;
            timeBtwAttack=startTimeBtwAttack;
            // El jugador salió del rango, reanudar el movimiento del AIPath.
            ResumePathfinding();
        }
    }*/
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
            moveSpeed = 2;
            health = 4;
            damage = 1.5f;

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

    void StopPathfinding()
    {
        aiPath.enabled = false;
    }

    void ResumePathfinding()
    {
        aiPath.enabled = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    void Attack()
    {
        //si se puede atacar
        if (timeBtwAttack <= 0)
        {
            anim.SetTrigger("Attack");
            //reset timer
            timeBtwAttack = startTimeBtwAttack;

        }
        else if (CanAttack)
        {
            timeBtwAttack -= Time.deltaTime;
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

    public void ReceiveDamage(float dmg)
    {
        TakeHit();
        health -= dmg;
        Debug.Log("Enemigo recibe " + dmg);
        //Debug.Log("Realizado: " + dmg + " de daño.");

        if (health <= 0)
        {
            PlayerData.xp += 50;
            if (PlayerData.currentVida < PlayerData.totalVida)
            {
                PlayerData.currentVida += PlayerData.currentLifeSteal;
            }
            anim.SetTrigger("Die");
            audioSource.Play();
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.enabled = false;
            Destroy(gameObject, 1f);
            PlayerData.contSuckler++;

            if (boss)
            {
                PlayerData.xp += 50;
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
