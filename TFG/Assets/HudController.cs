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
    public Image shieldp1;
    public Image shieldp2;

    public TextMeshProUGUI tagp1;
    public TextMeshProUGUI tagp2;
    public TextMeshProUGUI killstagp1;
    public TextMeshProUGUI killstagp2;
    public TextMeshProUGUI timertag;

    public BattleController battle;

    // Start is called before the first frame update
    void Start()
    {
        tagp1.text = player1.playerName;
        tagp2.text = player2.playerName;
        timertag.text = battle.timeLeft.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        timertag.text = battle.timeLeft.ToString();

        healthp1.fillAmount = player1.HealthPercent;
        if(rhealthp1.fillAmount < healthp1.fillAmount)
        {
            rhealthp1.fillAmount = healthp1.fillAmount;
        }
       
        if (rhealthp1.fillAmount >= player1.HealthPercent)
        {

            rhealthp1.fillAmount -= 0.003f;

        }


        healthp2.fillAmount = player2.HealthPercent;
        if (rhealthp2.fillAmount < healthp2.fillAmount)
        {
            rhealthp2.fillAmount = healthp2.fillAmount;
        }

        if (rhealthp2.fillAmount >= player2.HealthPercent)
        {
            
            rhealthp2.fillAmount -= 0.003f;

        }

        energyp1.fillAmount = player1.EnergyPercent;

        energyp2.fillAmount = player2.EnergyPercent;
      
        killstagp2.text = player2.timesKill.ToString();
 
        killstagp1.text = player1.timesKill.ToString();

        shieldp1.fillAmount = player1.ShieldPercent;

        shieldp2.fillAmount = player2.ShieldPercent;

    }
}
