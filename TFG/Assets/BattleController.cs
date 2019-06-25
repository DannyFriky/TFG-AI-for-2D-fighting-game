using System;
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
    public  CharacterAgent brain;
    public MLAgents.Brain brainP1, brainP2, brainP3;
    public MLAgents.Brain brainAt1, brainAt2, brainAt3;
    public MLAgents.Brain brainD1, brainD2, brainD3;
    private bool battleStarted= false;
    // Start is called before the first frame update
    void Start()
    {
        //Screen.fullScreen = true;
        Screen.SetResolution(ModeSelection.width , ModeSelection.height,ModeSelection.fullScreen);
      ModeChoosed();
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


        if (timeLeft == 0 || player1.HealthPercent == 0 || player2.HealthPercent == 0)
        {
            if (player1.HealthPercent == 0 || (timeLeft == 0 && player1.HealthPercent < player2.HealthPercent))
            {
                if (player2.player == Player.PlayerType.AI)
                {
                   banner.ShowYouDied();
                }
                else
                {
                    banner.ShowP2Win();
                }
            }
            if (player2.HealthPercent == 0 || (timeLeft == 0 && player1.HealthPercent > player2.HealthPercent))
            {
                if (player2.player == Player.PlayerType.AI)
                {
                    banner.ShowYouWin();

                }
                else
                {
                    banner.ShowP1Win();
                }
            }
            if (!player2.training)
                {
                    RestartGame();
                }
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

    public void ModeChoosed()
    {
        
        switch (ModeSelection.modeSelected)
        {
            case 0:
                brain.enabled = false;
                player2.player = Player.PlayerType.HUMAN2;
                break;
            case 1:
                brain.enabled = true;
                player2.player = Player.PlayerType.AI;
                brain.brain = brainP1;
                brain.difficulty = 0;
                break;
            case 2:
                brain.enabled = true;
                player2.player = Player.PlayerType.AI;
                brain.brain = brainP2;
                brain.difficulty = 0;
                break;
            case 3:
                brain.enabled = true;
                player2.player = Player.PlayerType.AI;
                brain.brain = brainP3;
                brain.difficulty = 0;
                break;
            case 4:
                brain.enabled = true;
                player2.player = Player.PlayerType.AI;
                brain.brain = brainAt1;
                brain.difficulty = 1;
                break;
            case 5:
                brain.enabled = true;
                player2.player = Player.PlayerType.AI;
                brain.brain = brainAt2;
                brain.difficulty = 1;
                break;
            case 6:
                brain.enabled = true;
                player2.player = Player.PlayerType.AI;
                brain.brain = brainAt3;
                brain.difficulty = 2;
                break;
            case 7:
                brain.enabled = true;
                player2.player = Player.PlayerType.AI;
                brain.brain = brainD1;
                brain.difficulty = 3;
                break;
            case 8:
                brain.enabled = true;
                player2.player = Player.PlayerType.AI;
                brain.brain = brainD2;
                brain.difficulty = 4;
                break;
            case 9:
                brain.enabled = true;
                player2.player = Player.PlayerType.AI;
                brain.brain = brainD3;
                brain.difficulty = 4;
                break;
            default:
                break;
        }
    }
}
