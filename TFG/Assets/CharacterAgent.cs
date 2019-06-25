using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CharacterAgent : Agent
{
    Rigidbody2D rBody;
    public BattleController battle;
    public Player character;
    int horizontalSignal = 0;
    int verticalSignal = 0;
    int horizontalDirection = 0;
    int verticalDirection = 0;
    int attackSignal = 0;
    bool attackAction;
    bool specialAction;
    private RayPerception2D rayPer;
    public float distanceToTarget;
    public int difficulty; // 0 = only walk
    public int lastTime;
    public float lastTimehp;
    public int nextAttack;
     

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        rayPer = GetComponent<RayPerception2D>();
        Monitor.SetActive(true);
        lastTime = battle.timeLeft;
        lastTimehp = character.health;
        nextAttack = lastTime;

    }

    public override void AgentReset()
    {
        if (difficulty == 0 && character.training)
        {
            character.oponent.transform.position = new Vector3(Random.Range(3f,17f) ,3.88f,0);
        }
        

        // battle.RestartGame();
    }

    public override void CollectObservations()
    {

        
        float rayDistance1 = 17f;
        float rayDistance2= 4f;
        float[] rayAngles1 = { 180f};
        float[] rayAngles2 = { 0f,90f,270f};
        string[] detectableObjects = { "Wall", "Player" };

        //Observations relative to the environment

        AddVectorObs(rayPer.Perceive(rayDistance1, rayAngles1, detectableObjects));
        AddVectorObs(rayPer.Perceive(rayDistance2, rayAngles2, detectableObjects));

        //Observations relative to own infirmation
        AddVectorObs(character.transform.position);
        AddVectorObs(character.HealthPercent);


        //Observations relative to oponent information
        AddVectorObs(character.oponent.HealthPercent);
        AddVectorObs(character.oponent.transform.position);
 
       
        if (difficulty == 2 || difficulty ==4)
        {
            AddVectorObs(character.oponent.attack);
            AddVectorObs(character.oponent.specialAttack);
        }



    }

    public void Update()
    {
        //Monitor.Log("Reward", GetCumulativeReward());
      // Debug.Log(GetCumulativeReward());

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        horizontalSignal = Mathf.FloorToInt(vectorAction[0]);
        verticalSignal = Mathf.FloorToInt(vectorAction[2]);
        attackSignal = Mathf.FloorToInt(vectorAction[1]);

        if (difficulty == 0)
        {
            verticalSignal = 0;
            attackSignal = 0;
        } else if (difficulty == 1 || difficulty == 2)
        {
            verticalSignal = 0;
        }
        switch (horizontalSignal)
        {
            case 1:
                horizontalDirection = 1; 
                break;
            case 2:
                horizontalDirection = -1;
                break;
            default:
                horizontalDirection = 0;
                break;
        }

        switch (verticalSignal)
        {
           case 1:
                verticalDirection = 1;
                break;
            case 2:

                if (difficulty > 3)
                {
                    if(character.oponent.attack || character.oponent.specialAttack)
                    {
                        verticalDirection = -1;

                    }
                } else
                {
                    verticalDirection = -1;

                }
                break;
            default:
                verticalDirection = 0;
                break;
        }


        switch (attackSignal)
        {

            case 1:
               
                attackAction = true;
                specialAction = false;

                break;
            case 2:
                specialAction = true;
                attackAction = false;

                break;
            default:
                attackAction = false;
                specialAction = false;
                break;
        }

        character.UpdateIA(horizontalDirection,verticalDirection,attackAction,specialAction);
        vectorAction[1] = 0;

        //rewards
        distanceToTarget = Vector3.Distance(character.transform.position, character.oponent.transform.position);

        if (difficulty == 0)
        {
            RewardByWalk();
            RewardByTime();
            if (distanceToTarget < 2f)
            {
                RewardByProximity();
                
            }
        }
        else if (difficulty == 1 || difficulty ==2)
        {
            RewardByWalk();
            RewardByTime();
            RewardByAttack();

            if (difficulty == 2)
            {
                RewardByDodge();

                if (character.training)
                {
                    AutoAttack();
                }
                
            }
            character.oponent.enemyAttack = false;
            character.oponent.specialAttack = false;

        }
        else if (difficulty >= 3)
        {
            RewardByWalk();
            RewardByTime();
            RewardByAttack();
            RewardByDodge();
            if (character.training)
            {
                AutoAttack();
            }

            RewardByDefend();

            character.oponent.attack = false;
            character.oponent.specialAttack = false;
        }



    }
       
    private void RewardByWalk()
    {
        if ((character.lookForward == false && character.horizontalMove > 0.01 && !character.attack && !character.defending) || (character.lookForward == true && character.horizontalMove < -0.01 && !character.attack && !character.defending))
        {
            if(distanceToTarget > 1)
            {
                SetReward(0.1f);

            }
        }

    }

    private void RewardByProximity()
    {
        SetReward(0.5f);
        Done();
    }

    private void RewardByTime()
    {
        if (lastTime > battle.timeLeft)
        {

            SetReward(-0.5f);
             lastTime = battle.timeLeft; 
            if (lastTime == 0 && character.training)
            {
                lastTime = 99;
                battle.timeLeft = 99;
                nextAttack = 99 - Random.Range(1, 2);
                character.shield = 100;

            }
        }
    }

    private void RewardByAttack()
    {

        if (character.oponent.enemyAttack == true && distanceToTarget < 3f)
        {
            SetReward(0.4f);
            if (character.training)
            {
                character.oponent.transform.position = new Vector3(Random.Range(3f, 17f), 3.88f, 0);

            }

        }

        if (character.oponent.health < 50 && character.training)
        {
            SetReward(1.0f);
            character.oponent.Heal();
            Done();
        }
    }

    private void RewardByDodge()
    {

        if (character.oponent.attack)
        {

            if ((character.lookForward == false && character.horizontalMove < 0.01) || (character.lookForward == true && character.horizontalMove > -0.01))
            {
                SetReward(0.01f);

            }
            if (lastTimehp < character.health)
            {
                lastTimehp = character.health;
                SetReward(-0.4f);
            }
            if (lastTimehp == character.health)
            {
                SetReward(0.01f);

            }

            if (character.health < 50 && character.training)
            {
                SetReward(-1.0f);
                character.Heal();
            }

        }


    }

    private void AutoAttack()
    {
        if (battle.timeLeft == nextAttack)
        {
            nextAttack = battle.timeLeft - Random.Range(1, 2);
            if (distanceToTarget < 3.0f)
            {
                character.oponent.ForcedAttack();
            }

            if (distanceToTarget > 6.0f)
            {
                character.oponent.ForcedSAttack();
            }
        }
    }

    private void RewardByDefend()
    {
        if (character.defending == true && character.oponent.attack && distanceToTarget < 3f)
        {
            SetReward(0.3f);

        }

        if (character.defending == true && character.oponent.specialAttack)
        {
            SetReward(0.3f);
        }

        if (character.shield == 0)
        {
            SetReward(0.5f);
        }


    }
}
