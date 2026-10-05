using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //STATS
    public static float totalVida = 50;
    public static float currentVida = 50;
    public static float maxVida = 100;// 5 cada nivel

    public static float currentArmadura=0.05f;
    public static float maxArmadura=0.4f; //0.035 cada nivel

    public static float currentDamage = 1;
    public static float maxDmg = 6; // 0.5 cada nivel

    public static float currentLifeSteal = 0.5f;
    public static float maxLifeSteal = 2; // 0.15 cada nivel

    public static float currentVelocidadDash= 20; // 1 cada nivel
    public static float MaxVelocidadDash = 30;

    public static float currentVelocidadMovimiento = 8; //0.4 cada nivel 
    public static float maxVelocidadDeMovimiento = 20;

    public static int contBase=0;
    public static int contSuckler=0;
    public static int contWizzard=0;
    public static int contScorpion = 0;
    public static int contKeys = 0;




    public static float fuerzaDeSalto = 8;

    public static int puntos = 0;
    
    public static float xp=0;
    public static int lvl=1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
