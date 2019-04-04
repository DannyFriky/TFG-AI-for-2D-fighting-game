using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static int MAX_TIME = 99;
    public int timeLeft = MAX_TIME;
    private float lastTimeUpdate = 0;
    public Player player1;
    public Player player2;
    public Vector3 posRel;
    public AudioSource musicPlayer;
    public AudioClip backgroundMusic;
    public BannerController banner;
    private bool battleStarted= false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (battleStarted == false)
        {
            battleStarted = true;
            GameUtils.PlaySound(backgroundMusic, musicPlayer);
        }
        if(timeLeft>0 && Time.time - lastTimeUpdate > 1)
        {
            timeLeft--;
            lastTimeUpdate = Time.time;
        }

       Vector3 aToB = player2.transform.position - player1.transform.position;

       posRel = player1.transform.InverseTransformPoint(aToB);

        if(aToB.x < 0)
        {
            player1.FlipSprite(1);
            player2.FlipSprite(1);

        } else
        {
            player1.FlipSprite(0);
            player2.FlipSprite(0);
        }

        
        //Will restar the battle forever until training its done
        if(timeLeft == 0 || player1.HealthPercent == 0 || player2.HealthPercent == 0)
        {
            if (player1.HealthPercent == 0)
            {
                banner.ShowYouDie();
            }
            RestartGame();
        }
    }

    public void RestartGame()
    {
       

        //yield return new WaitForSeconds(0);
        if (player1.HealthPercent > player2.HealthPercent)
        {
            player1.GetKill();
        }

        if (player1.HealthPercent < player2.HealthPercent)
        {
            player2.GetKill();
        }
        timeLeft = MAX_TIME;
        player1.Restart();
        player2.Restart();
        

    }
}
