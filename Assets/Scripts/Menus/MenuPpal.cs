using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPpal : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void NewGame(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
    }
    public void Retry()
    {
        SceneManager.LoadScene("Nivel_1");
    }
}
