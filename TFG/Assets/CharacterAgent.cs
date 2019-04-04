using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CharacterAgent : Agent
{
    Rigidbody2D rBody;
    public BattleController battle;
    public Player character;
    public Animator animator;
    int horizontalSignal = 0;
    int horizontalDirection = 0;
    int atackSignal = 0;
    bool atackAction;
    RayPerception rayPer;
    //readonly string[] detectableObjects = { "Wall", "Player", "EnergySphereP1", "EnergySphereP2" };
    //readonly float rayDistance = 20.0f;
   // readonly float[] rayAngles = { 1f, 45f, 90f, 135f, 180f, 110f, 70f };

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        rayPer = GetComponent<RayPerception>();
        Monitor.SetActive(true);
        

    }

    public override void AgentReset()
    {
            character.oponent.transform.position = new Vector3(Random.Range(3f,17f) ,3.88f,0);
        battle.RestartGame();
    }

    public override void CollectObservations()
    {
        //Observations relative to the environment
   

        AddVectorObs(battle.timeLeft);
        //No puedo usar raycasting con 2D todavia, han dicho que en unas semanas estara, ir revisando
        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0.0f, 0.0f));

        //Observations relative to own infirmation
        AddVectorObs(character.transform.position);
        AddVectorObs(character.HealthPercent);
        AddVectorObs(character.ShieldPercent);
        AddVectorObs(character.EnergyPercent);


        //Observations relative to oponent information
        AddVectorObs(character.oponent.HealthPercent);
        AddVectorObs(character.oponent.transform.position);
        AddVectorObs(character.oponent.ShieldPercent);
        AddVectorObs(character.oponent.EnergyPercent);
        AddVectorObs(character.oponent.CurrentAction);



    }
    public void Update()
    {
        Monitor.Log("Reward", GetCumulativeReward());
       // Debug.Log(GetCumulativeReward());

    }
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        horizontalSignal = Mathf.FloorToInt(vectorAction[0]);
        atackSignal = Mathf.FloorToInt(vectorAction[1]);
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

        switch (atackSignal)
        {
            case 1:
                atackAction = true;
                
                break;
            default:
                atackAction = false;
                break;
        }

        character.UpdateIA(horizontalDirection,atackAction);
        vectorAction[1] = 0;// Para que cada orden de atacar solo cuente 1 vez, si quiere volver a atacar deberia volver a mandar la orden de ataque (?)

        //rewards
        float distanceToTarget = Vector3.Distance(character.transform.position, character.oponent.transform.position);

        if( character.startAtack == true && distanceToTarget > 3f)
        {
            SetReward(-0.1f);
            character.startAtack = false;
        }

        if (character.oponent.HealthPercent < character.HealthPercent)
        {
            SetReward(0.5f);
            character.oponent.Heal();
            character.oponent.transform.position = new Vector3(Random.Range(3f, 17f), 3.88f, 0);

        }
        if ((character.lookFoward == false && character.horizontalMove > 0.01) || (character.lookFoward == true && character.horizontalMove < -0.01))
        {
            SetReward(0.01f);
        }
        if ((character.lookFoward == false && character.horizontalMove < -0.01) || (character.lookFoward == true && character.horizontalMove > 0.01))
        {
            SetReward(-0.01f);
        }

        if (battle.timeLeft == 1)
        {
           
            Done();
        }
    }



}
