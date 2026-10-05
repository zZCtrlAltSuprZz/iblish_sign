using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text lvl;
    [SerializeField] TMP_Text xp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lvl.text=PlayerData.lvl.ToString()+" / 26";
        xp.text=PlayerData.xp.ToString() + " / 300";
    }
}
