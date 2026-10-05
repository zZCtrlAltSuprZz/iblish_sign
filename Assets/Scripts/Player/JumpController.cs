using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JumpController : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;

    Rigidbody2D rb;
    Animator anim;
    [SerializeField]BoxCollider2D boxColl;

    [SerializeField] AudioSource aSource;



    private void Start()
    {
        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();

    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.5f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);
        anim.SetBool("Jumping", !isGrounded);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            aSource.Play();
            rb.velocity = new Vector2(rb.velocity.x, PlayerData.fuerzaDeSalto);
            rb.AddForce( new Vector2(rb.velocity.x, PlayerData.fuerzaDeSalto),ForceMode2D.Impulse);

        }


    }
}
