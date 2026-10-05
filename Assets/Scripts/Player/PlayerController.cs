using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Image imgBarraVida;
    [SerializeField] AudioSource aSource;


    private float movementInputDirection;
    private bool isFacingRight = true;
    private bool isRunning;

    //ANIM & RB
    private Animator anim;
    private Rigidbody2D rb;

    //DASH//
    private float cooldownTime = 3f;    // Tiempo de enfriamiento en segundos
    private float timerCd;           // Temporizador para rastrear el tiempo transcurrido
    [SerializeField] private float dashTime;
    private float initialGravity;
    private bool canDash=true;
    private bool canMove=true;
    [SerializeField] private TrailRenderer trailRenderer;

    //Recibir daño color de sprites
    public SpriteRenderer spriteRenderer;
    public Color hitColor;
    public float hitDuration = 0.1f;
    public int numberOfHits = 3; // Número de veces que parpadeará al ser golpeado

    private Color originalColor;


    // Start is called before the first frame update
    void Start()
    {
        originalColor = spriteRenderer.color;
        Time.timeScale = 1.0f;
        timerCd = 3f;
        rb= GetComponent<Rigidbody2D>();    
        anim= GetComponent<Animator>();
        initialGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {

       
        //
        //textPuntAlmas.text = "" + PlayerData.puntos;
        //
        CheckInput();
        CheckMovementDirection();
        //vidaPlayer = PlayerData.vida;
        imgBarraVida.fillAmount = PlayerData.currentVida / PlayerData.totalVida;
        
        // Actualiza el temporizador en cada frame

        timerCd += Time.deltaTime;

        // Verifica si ha pasado el tiempo de enfriamiento
        if (timerCd >= cooldownTime)
        {
            if (canDash && Input.GetKeyDown(KeyCode.Space))
            {
                aSource.Play();
                //Debug.Log("Dasheando");
                StartCoroutine(Dash());
            }
            
        }
        

    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Movement();
        }

        

        UpdateAnimations();

    }

   

    private IEnumerator Dash()
    {
        timerCd = 0f;
        canMove = false;
        canDash = false;

        //se quita la gravedad
        rb.gravityScale = 0;

        //si mira a la derecha mientras dashea
        if(isFacingRight)
        {
            anim.SetTrigger("Dash");
            trailRenderer.emitting = true;
            rb.velocity = new Vector2(PlayerData.currentVelocidadDash, 0);
        }
        //si mira a la izquierda mientras dashea
        else
        {
            anim.SetTrigger("Dash");
            trailRenderer.emitting = true;
            rb.velocity = new Vector2(-PlayerData.currentVelocidadDash, 0);
        }

        yield return new WaitForSeconds(dashTime);

        canMove = true;
        canDash = true;
        rb.gravityScale = initialGravity;
        trailRenderer.emitting = false;

    }

    private void CheckMovementDirection()
    {
       

        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        
    }

    //private void Aceleracion()
    //{

    //    if (movementInputDirection != 0 && movementSpeed < movementSpeedMax)
    //    {
    //        movementSpeed += aceleration * Time.fixedDeltaTime;
    //    }
    //    if (movementInputDirection == 0 && movementSpeed < 10)
    //    {
    //        movementSpeed -= deceleration * Time.fixedDeltaTime;
    //    }

    //    movementSpeed = Mathf.Clamp(movementSpeed, 0, movementSpeedMax);

    //}
    
    public void RecieveDamage(float _dmg)
    {
        TakeHit();
        PlayerData.currentVida -= _dmg;
        Debug.Log("Player recibe " + _dmg + " de daño.");
        if (PlayerData.currentVida<=0)
        {
            SceneManager.LoadScene("DeathScreen");

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
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
    }

    private void Movement()
    {
        if (movementInputDirection != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        
        if (movementInputDirection!=0)
        {
            rb.velocity = new Vector2(PlayerData.currentVelocidadMovimiento * movementInputDirection , rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2 (0, rb.velocity.y);
        }

        PlayerData.currentVelocidadMovimiento = Mathf.Clamp(PlayerData.currentVelocidadMovimiento, 0, PlayerData.maxVelocidadDeMovimiento);

    }

    private void UpdateAnimations()
    {
        anim.SetBool("Running", isRunning);
    }


   
}
