using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public Player player1;
    public Player player2;


    public Image healthp1;
    public Image healthp2;
    public Image rhealthp1;
    public Image rhealthp2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        healthp1.fillAmount = player1.HealthPercent;


        if (rhealthp1.fillAmount >= player1.HealthPercent)
        {

            rhealthp1.fillAmount -= 0.003f;

        }


        healthp2.fillAmount = player2.HealthPercent;
 
        if (rhealthp2.fillAmount >= player2.HealthPercent)
        {
            
            rhealthp2.fillAmount -= 0.003f;

        }

    }
}
