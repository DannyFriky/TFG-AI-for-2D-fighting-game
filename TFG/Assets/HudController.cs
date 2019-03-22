using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
    public Player player1;
    public Player player2;


    public Image healthp1;
    public Image healthp2;
    public Image rhealthp1;
    public Image rhealthp2;
    public Image energyp1;
    public Image energyp2;

    public TextMeshProUGUI tagp1;
    public TextMeshProUGUI tagp2;


    // Start is called before the first frame update
    void Start()
    {
        tagp1.text = player1.playerName;
        tagp2.text = player2.playerName;

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

        energyp1.fillAmount = player1.EnergyPercent;

        energyp2.fillAmount = player2.EnergyPercent;
    }
}
