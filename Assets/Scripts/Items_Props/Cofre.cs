using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cofre : MonoBehaviour
{
    Animator anim;
    bool insideChest;
    bool opened = false;

    [SerializeField] GameObject cofreText;

    [SerializeField] AudioSource aSource;


    void Start()
    {
        cofreText.SetActive(false);
        insideChest = false;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (insideChest && !opened)
        {
            cofreText.SetActive(true);
        }
        else
        {
            cofreText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && insideChest && !opened)
        {
            aSource.Play();
            opened = true;
            cofreText.SetActive(false);
            anim.SetTrigger("Open");
            PlayerData.xp += Random.Range(100, 300);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !opened)
        {
            insideChest = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !opened)
        {
            insideChest = false;
        }
    }
}
