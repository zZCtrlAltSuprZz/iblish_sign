using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuXpRamas : MonoBehaviour
{
    bool activo = false;

    [SerializeField] TMP_Text textLvl;
    [SerializeField] TMP_Text textLvlUp;



    [SerializeField] TMP_Text agilityLvl;
    [SerializeField] TMP_Text textMoviSpeed;
    [SerializeField] TMP_Text textDashSpeed;
    public int agility_LvlNumber=0;

    [SerializeField] TMP_Text assasinLvl;
    [SerializeField] TMP_Text textDamage;
    [SerializeField] TMP_Text textLifeSteal;
    public int assasin_LvlNumber=0;

    [SerializeField] TMP_Text tankLvl;
    [SerializeField] TMP_Text textHealth;
    [SerializeField] TMP_Text textArmor;
    public int tank_LvlNumber=0;


    [SerializeField] GameObject panelXP;

    float xpSiguienteNivel=250;
    bool puedeSubirNivel=false;


    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



        if (PlayerData.xp >=xpSiguienteNivel)
        {
            textLvlUp.enabled = true;
            puedeSubirNivel = true;
        }
        else
        {
            textLvlUp.enabled=false;
            puedeSubirNivel = false;
        }



        //datos por pantalla
        textLvl.text = PlayerData.lvl.ToString();

        //nivel de ramas en pantalla 
        agilityLvl.text = agility_LvlNumber.ToString() + "/ " + 10;
        tankLvl.text = tank_LvlNumber.ToString() + "/ " + 10;
        assasinLvl.text = assasin_LvlNumber.ToString() + "/ " + 10;
        
        //stats en pantalla 
        textMoviSpeed.text = PlayerData.currentVelocidadMovimiento.ToString("F1");
        textDashSpeed.text = PlayerData.currentVelocidadDash.ToString();
        textDamage.text = (PlayerData.currentDamage*5).ToString();
        textLifeSteal.text = (PlayerData.currentLifeSteal*10).ToString("F1");
        textArmor.text = (PlayerData.currentArmadura*100).ToString();
        textHealth.text = PlayerData.currentVida.ToString();



    }

    public void ActPanelXp()
    {
        if (activo)
        {
            Time.timeScale = 1;
            activo= false;
            panelXP.SetActive(false);
        }
        else
        {
            activo = true;
            Time.timeScale = 0;
            panelXP.SetActive(true);
        }
    }

    public void Agility_lvl_up()
    {
        if (puedeSubirNivel && agility_LvlNumber<10 && PlayerData.lvl <27)
        {
            PlayerData.currentVelocidadMovimiento += 0.4f;
            PlayerData.currentVelocidadDash += 1f;
            PlayerData.lvl += 1;
            PlayerData.xp -= 200;

            agility_LvlNumber += 1;
        }
        
    }

    public void Assasin_lvl_up()
    {
        if (puedeSubirNivel && assasin_LvlNumber < 10 && PlayerData.lvl < 27)
        {
            PlayerData.currentDamage += 0.5f;
            PlayerData.currentLifeSteal += 0.15f;
            PlayerData.lvl += 1;
            PlayerData.xp -= 200;

            assasin_LvlNumber += 1;
        }
    }

    public void Tank_lvl_up()
    {
        if (puedeSubirNivel && tank_LvlNumber < 10 && PlayerData.lvl < 27)
        {
            PlayerData.currentArmadura += 0.035f;
            PlayerData.totalVida += 5f;
            PlayerData.currentVida += 5f;
            PlayerData.lvl += 1;
            PlayerData.xp -= 200;

            tank_LvlNumber += 1;
        }
    }


}
