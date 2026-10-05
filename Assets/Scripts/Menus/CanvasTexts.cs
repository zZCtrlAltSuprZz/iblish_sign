using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasTexts : MonoBehaviour
{
    [SerializeField] TMP_Text contSuckler;
    [SerializeField] TMP_Text contBase;
    [SerializeField] TMP_Text contWizzard;
    [SerializeField] TMP_Text contScorpion;
    [SerializeField] TMP_Text origin;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        contSuckler.text = PlayerData.contSuckler.ToString() + " of 15 sucklers killed";
        contBase.text = PlayerData.contBase.ToString() + " of 15 seekers killed";
        contWizzard.text = PlayerData.contWizzard.ToString() + " of 15 wizzards killed";
        contScorpion.text = PlayerData.contScorpion.ToString() + " of 15 scorpions killed";

        if (PlayerData.contKeys <4)
        {
            origin.text = PlayerData.contKeys.ToString() + " of 4 keys";
        }else if (PlayerData.contKeys >= 4)
        {
            origin.text = "You got the 4 keys! Press E button!";
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("Cinematica");
            }
        }
    }
}
