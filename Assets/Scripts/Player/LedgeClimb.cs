using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeClimb : MonoBehaviour
{

    public LayerMask ledgeLayerMask;
    public Transform initialRayPosition;
    bool agarrado = false;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float rayDistance=1.5f;
     float jumpPower = 8f;
    Animator anim;
    [SerializeField]float tiempoParaAgarre=0.1f;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(initialRayPosition.position, transform.right * rayDistance, Color.red);

        if (agarrado==true)
        {
            anim.SetTrigger("Colgado");
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            if (Input.GetKeyDown(KeyCode.W))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                rb.AddForce(new Vector2(rb.velocity.x, jumpPower), ForceMode2D.Impulse);
                agarrado=false;
                tiempoParaAgarre = 0.1f;
                rb.gravityScale = 3;
            }else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
            {
                agarrado = false;
                rb.gravityScale = 3;
            }
        }
        else
        {
            if (tiempoParaAgarre > 0)
            {
                tiempoParaAgarre -= Time.deltaTime;
            }

            if (tiempoParaAgarre <= 0)
            {

                DetectLedge();

            }
            
            
        }
        
    }
    void DetectLedge()
    {
        if (agarrado==false)
        {
            // Realiza un raycast hacia abajo desde la posición del personaje
            RaycastHit2D hit = Physics2D.Raycast(initialRayPosition.position, transform.right, rayDistance, ledgeLayerMask);
            if (hit.collider != null)
            {    
                if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Ledge")
                {
                    agarrado = true;
                    // Aquí podrías agregar la lógica de escalada cuando se detecta el ledge
                }
            }
        }
        
        
    }

}
