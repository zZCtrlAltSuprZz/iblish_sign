using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackController : MonoBehaviour
{

    [SerializeField] AudioSource aSource;
    [SerializeField] AudioClip att1Clip;
    [SerializeField] AudioClip att2Clip;



    [SerializeField] float temporizadorAttack, temporizadorAttack2, temporizadorTotalAttack = 1.5f;
    float temporizadorMaxAttack = 0.5f, temporizadorMaxAttack2 = 0.5f;

    [SerializeField] int state=0;

    private Animator anim;

    public LayerMask enemieMask;

    public GameObject attackCenter;
    public GameObject attackCenter2;

    float radiusFirstAtt = 1;
    float radiusSecondAtt= 1.8f;

    public GameObject hitParticles; // Referencia al prefab del sistema de partículas

    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        temporizadorAttack = temporizadorMaxAttack;
        temporizadorAttack2 = temporizadorMaxAttack2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Attack();
        
    }
    public void SonidoEspada1()
    {
        aSource.PlayOneShot(att1Clip);   
    }
    public void SonidoEspada2()
    {
        aSource.PlayOneShot(att2Clip);
    }

    // Método para activar el sistema de partículas durante el ataque
    public void ActivateParticleSystem(Transform transformPoint)
    {
        // Instanciar el prefab del sistema de partículas en la posición y rotación del enemigo
        GameObject particleSystemInstance = Instantiate(hitParticles, transformPoint);
        particleSystemInstance.transform.position = transformPoint.position;
        // Asegurarse de detener o destruir el sistema de partículas después de cierto tiempo
        Destroy(particleSystemInstance, particleSystemInstance.GetComponent<ParticleSystem>().main.duration);
    }

    public void DetectEnemyAndHit()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackCenter.transform.position, radiusFirstAtt, enemieMask);

        // Verificar si hay enemigos en el área
        if (enemies != null && enemies.Length > 0)
        {
            // Iterar a través de todos los enemigos
            foreach (Collider2D enemyCollider in enemies)
            {
                if (enemyCollider != null)
                {
                    // Obtener el componente EnemyBaseBehaviour del enemigo

                    if (enemyCollider.gameObject.CompareTag("EnemigoVolador"))
                    {
                        //particulas
                        ActivateParticleSystem(enemyCollider.gameObject.transform);
                        // Aplicar daño al enemigo
                        //enemyCollider.GetComponent<SucklerController>().ReceiveDamage(PlayerData.dmg);
                        enemyCollider.GetComponentInChildren<SucklerController>().ReceiveDamage(PlayerData.currentDamage);
                    }
                    // Verificar si el componente existe antes de intentar acceder a sus funciones
                    if (enemyCollider.gameObject.CompareTag("EnemigoBase"))
                    {
                        //particulas
                        ActivateParticleSystem(enemyCollider.gameObject.transform);
                        // Aplicar daño al enemigo
                        enemyCollider.GetComponent<SeekerController>().ReceiveDamage(PlayerData.currentDamage);

                     
                    }
                    if (enemyCollider.gameObject.CompareTag("Wizzard"))
                    {
                        //particulas
                        ActivateParticleSystem(enemyCollider.gameObject.transform);
                        // Aplicar daño al enemigo
                        enemyCollider.GetComponent<WizzardIA>().ReceiveDamage(PlayerData.currentDamage);


                    }
                    if (enemyCollider.gameObject.CompareTag("Scorpion"))
                    {
                        //particulas
                        ActivateParticleSystem(enemyCollider.gameObject.transform);
                        // Aplicar daño al enemigo
                        enemyCollider.GetComponent<ScorpionIA>().ReceiveDamage(PlayerData.currentDamage);


                    }
                }
            }
        }
    }
    public void detectEnemyAndHit2()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackCenter2.transform.position, radiusSecondAtt, enemieMask);

        // Verificar si hay enemigos en el área
        if (enemies != null && enemies.Length > 0)
        {
            // Iterar a través de todos los enemigos
            foreach (Collider2D enemyCollider in enemies)
            {
                if (enemyCollider != null)
                {
                    // Obtener el componente EnemyBaseBehaviour del enemigo

                    // Verificar si el componente existe antes de intentar acceder a sus funciones
                    if (enemyCollider.gameObject.CompareTag("EnemigoVolador"))
                    {

                        //particulas
                        ActivateParticleSystem(enemyCollider.gameObject.transform);
                        // Aplicar daño al enemigo
                        
                        //enemyCollider.GetComponent<SucklerController>().ReceiveDamage(PlayerData.dmg * 2);
                        enemyCollider.GetComponentInChildren<SucklerController>().ReceiveDamage(PlayerData.currentDamage*2);

                        
                    }
                    // Verificar si el componente existe antes de intentar acceder a sus funciones
                    if (enemyCollider.gameObject.CompareTag("EnemigoBase"))
                    {
                        //particulas
                        ActivateParticleSystem(enemyCollider.gameObject.transform);
                        // Aplicar daño al enemigo
                        enemyCollider.GetComponent<SeekerController>().ReceiveDamage(PlayerData.currentDamage*2);
                    }

                    if (enemyCollider.gameObject.CompareTag("Wizzard"))
                    {
                        //particulas
                        ActivateParticleSystem(enemyCollider.gameObject.transform);
                        // Aplicar daño al enemigo
                        enemyCollider.GetComponent<WizzardIA>().ReceiveDamage(PlayerData.currentDamage * 2);


                    }
                    if (enemyCollider.gameObject.CompareTag("Scorpion"))
                    {
                        //particulas
                        ActivateParticleSystem(enemyCollider.gameObject.transform);
                        // Aplicar daño al enemigo
                        enemyCollider.GetComponent<ScorpionIA>().ReceiveDamage(PlayerData.currentDamage * 2);


                    }
                }
            }
        }
    }
    public void Attack()
    {
        
        if (state==0)
        {
            
            anim.SetBool("Attack", false);
            anim.SetBool("SecondAttack", false);

            if (Input.GetButton("Fire1") && !EventSystem.current.IsPointerOverGameObject())
            {
                SonidoEspada1();
                anim.SetBool("Attack", true);
                state = 1;
            }
            
        }

        if (state==1)
        {
            temporizadorAttack -= Time.fixedDeltaTime;

            temporizadorTotalAttack -= Time.fixedDeltaTime;

            if (temporizadorAttack <= 0.1f)
            {
                anim.SetBool("Attack", false);
                anim.SetBool("SecondAttack", false);
            }

            if (temporizadorTotalAttack <= 0)
            {
                temporizadorAttack = temporizadorMaxAttack;
                temporizadorTotalAttack = 1.5f;
                state = 0;




            }

            if (Input.GetButton("Fire1") && temporizadorTotalAttack>=0 && temporizadorTotalAttack<=1f)
            {
                SonidoEspada2();
                state = 2;
                temporizadorAttack = temporizadorMaxAttack;
                temporizadorTotalAttack = 1.5f;

            }
            
            
        }

        if (state == 2)
        {
            anim.SetBool("SecondAttack", true);
            temporizadorAttack2 -= Time.fixedDeltaTime;
            if (temporizadorAttack2 <= 0)
            {
                temporizadorAttack2 = temporizadorMaxAttack2;
                temporizadorTotalAttack = 1.5f;
                state = 0;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCenter.transform.position, radiusFirstAtt);
        Gizmos.DrawWireSphere(attackCenter2.transform.position, radiusSecondAtt);


    }
}
