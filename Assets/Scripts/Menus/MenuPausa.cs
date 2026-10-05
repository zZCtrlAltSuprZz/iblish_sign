using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPausa : MonoBehaviour
{

    public GameObject objMenuPausa;
    public bool pausa=false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActDesMenuPausa();
        }
    }

    public void ActDesMenuPausa()
    {
        if (pausa == false)
        {
            objMenuPausa.SetActive(true);
            pausa = true;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            objMenuPausa.SetActive(false);
            pausa = false;
            Time.timeScale = 1;
        }
    }

    public void ResumeGame()
    {
        objMenuPausa.SetActive(false);
        pausa = false;
        Time.timeScale = 1;

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuPpal");
    }


}
