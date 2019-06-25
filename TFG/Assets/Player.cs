using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerType
    {
        HUMAN, AI, HUMAN2
    };



    public static float MAX_HEALTH = 100f;
    public static float MAX_ENERGY = 300f;
    public static float MAX_SHIELD = 100f;

    public CharacterController2D controller;
    public Animator animator;
    public float health = MAX_HEALTH;
    public float energy = 0;
    public float shield = MAX_SHIELD;
    public int timesKill = 0;
    public string playerName;
    public Player oponent;
    public PlayerType player;
    public EnergySphere sphere;
    public SpriteRenderer spriteRendered;
    private Rigidbody2D myBody;
    private AudioSource audioPlayer;

    public AudioClip attack1s;
    public AudioClip attack2s;
    public AudioClip attack3s;
    public AudioClip specials;
    public AudioClip dmghits;
    public AudioClip shieldhits;
    public AudioClip shieldbreaks;
    public AudioClip jumps;
    public bool throwForward;




    public float runSpeed = 40f;

    public float horizontalMove = 0f;
    public bool crouch = false;
    public bool jump = false;
    public bool attack = false;
    public bool lookForward = true;
    public bool defending = false;
    public bool dead = false;
    public bool onLand = true;
    public bool enemyAttack = false;
    public bool specialAttack = false;
    public bool training;


    Vector3 initialPosition;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialPosition = this.transform.position;
        audioPlayer = GetComponent<AudioSource>();
      
    }

    public void UpdateHumanInput ()
    {
        if (!PauseMenu.GameIsPaused)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Movement", horizontalMove);
            animator.SetFloat("Health", HealthPercent);



            if ((horizontalMove > 0.1 && lookForward == true) || (horizontalMove < -0.1 && lookForward == false))
            {
                animator.SetBool("Walk_Forward", true);
            }
            else
            {
                animator.SetBool("Walk_Forward", false);
            }
            if ((horizontalMove < -0.1 && lookForward == true) || (horizontalMove > 0.1 && lookForward == false))
            {
                animator.SetBool("Walk_Backwards", true);
            }
            else
            {
                animator.SetBool("Walk_Backwards", false);
            }
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                onLand = false;
                animator.SetBool("OnAir", true);
            }

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
                animator.SetBool("Is_Crouching", true);
                if (shield > 0)
                {
                    defending = true;
                    animator.SetBool("Defending", true);
                   

                }
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                crouch = false;
                animator.SetBool("Is_Crouching", false);
                defending = false;
                animator.SetBool("Defending", false);

            }
            if (Input.GetButtonDown("Attack"))
            {
                if (!specialAttack)
                {
                    animator.SetTrigger("Attack");
                }

            }
            if (Input.GetButtonDown("SpecialAttack"))
            {
                if (energy >= 100)
                {
                    specialAttack = true;
                    animator.SetTrigger("Special_Attack");
                    energy -= 100;
                }
            }
        }
        
    } 

    public void UpdateHuman2Input()
    {
        if (!PauseMenu.GameIsPaused)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal2") * runSpeed;
            animator.SetFloat("Movement", horizontalMove);

            if ((horizontalMove < -0.1 && lookForward == true) || (horizontalMove > 0.1 && lookForward == false))
            {
                animator.SetBool("Walk_Forward", true);
            }
            else
            {
                animator.SetBool("Walk_Forward", false);

            }
            if ((horizontalMove > 0.1 && lookForward == true) || (horizontalMove < -0.1 && lookForward == false))
            {
                animator.SetBool("Walk_Backwards", true);
            }
            else
            {
                animator.SetBool("Walk_Backwards", false);

            }
            if (Input.GetButtonDown("Jump2"))
            {
                jump = true;
                onLand = false;

                animator.SetBool("OnAir", true);
            }

            if (Input.GetButtonDown("Crouch2"))
            {
                crouch = true;
                animator.SetBool("Is_Crouching", true);
                if (shield > 0)
                {
                    defending = true;
                    animator.SetBool("DefendingH", true);

                }
            }
            else if (Input.GetButtonUp("Crouch2"))
            {
                crouch = false;
                animator.SetBool("Is_Crouching", false);
                defending = false;
                animator.SetBool("DefendingH", false);

            }
            if (Input.GetButtonDown("Attack2"))
            {
                animator.SetTrigger("Attack");
            }
            if (Input.GetButtonDown("SpecialAttack2"))
            {
                if (energy >= 100)
                {
                    animator.SetTrigger("Special_Attack");
                    energy -= 100;
                }
            }
        }
    }

    public void UpdateIA(int horizontalDirection,int verticalDirection, bool attackAction, bool specialAction)
    {

        horizontalMove = horizontalDirection * runSpeed;
        animator.SetFloat("Movement", horizontalMove);


        if (((horizontalMove < -0.1 && lookForward == true) || (horizontalMove > 0.1 && lookForward == false)))
        {
            animator.SetBool("Walk_Forward", true);
        }
        else
        {
            animator.SetBool("Walk_Forward", false);

        }
        if (((horizontalMove > 0.1 && lookForward == true) || (horizontalMove < -0.1 && lookForward == false)))
        {
            animator.SetBool("Walk_Backwards", true);
        }
        else
        {
            animator.SetBool("Walk_Backwards", false);

        }

        if (attackAction)
        {
            if (!specialAttack && !defending)
            {
                animator.SetTrigger("Attack");
            }
        }

        if (specialAction)
        {
            if (energy >= 100)
            {
                specialAttack = true;
                animator.SetTrigger("Special_Attack");
                energy -= 100;
            }
        }

       /* if (verticalDirection == 1)
        {

            jump = true;
            onLand = false;

            animator.SetBool("OnAir", true);
        }*/
        if (verticalDirection == -1)
        {

            
           if (shield > 0 && !defending && !specialAttack && !attack)
           {
                    animator.SetBool("Is_Crouching", true);
                    animator.SetBool("Defending", true);

           }
        

        }



    }

    public void Update()
    {


        if (oponent != null)
        {
            animator.SetFloat("Oponent_Health", oponent.HealthPercent);
        } else
        {
            animator.SetFloat("Oponent_Health", 1);
        }

        if (dead == false)
        {
            if (player == PlayerType.HUMAN)
            {
                UpdateHumanInput();
            }
            if (player == PlayerType.HUMAN2)
            {
                UpdateHuman2Input();
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("OnAir", false);
        onLand = true;
    }

    public void StartAttack()
    {
        attack = true;

    }

    public void EndAttack()
    {
        attack = false;
    }

    public void StartSpecialAttack()
    {
        attack = true;

    }

    public void EndSpecialAttack()
    {
        specialAttack = false;
    }

    public void ThrowSphere()
    {
        Vector3 SphereInitialPosition;

        if ((lookForward == false && this.player == PlayerType.HUMAN) || (lookForward == true && (this.player == PlayerType.HUMAN2 || this.player == PlayerType.AI)))
        {
            SphereInitialPosition = new Vector3(this.transform.position.x -1, this.transform.position.y, 0);
            throwForward = false;
        }
        else
        {
            SphereInitialPosition = new Vector3(this.transform.position.x +1, this.transform.position.y, 0);
            throwForward = true;
   
        }
        EnergySphere clone = EnergySphere.Instantiate(
            sphere,
            SphereInitialPosition,
            Quaternion.Euler(0,0,0));
        clone.caster = this;
    }

    public void StartDefense()
    {
        

        crouch = true;
        defending = true;
    }

    public void EndDefense()
    {
      
        if (player == PlayerType.AI)
        {
            crouch = false;

            defending = false;
            animator.SetBool("Is_Crouching", false);
            animator.SetBool("Defending", false);
        }


    }
    
    public void DamageReceived(float damage)
    {
        if (dead == false)
        {
            this.UpdateEnergy(35);
            if (defending == true && shield > 0)
            {
                GameUtils.PlaySound(shieldhits, audioPlayer);

                if (shield >= damage)
                {
                    shield -= damage;
                    if (shield == 0)
                    {
                        animator.SetBool("Defending", false);
                        GameUtils.PlaySound(shieldbreaks, audioPlayer);

                    }
                }
                else
                {
                    health = health - (damage - shield);
                    shield = 0;
                    animator.SetBool("Defending", false);
                    GameUtils.PlaySound(shieldbreaks, audioPlayer);

                }
            }
            else
            {
                GameUtils.PlaySound(dmghits, audioPlayer);

                if (health >= damage)
                {
                    health -= damage;

                    if (health == 0 && !training)
                    {
                        
                        animator.SetTrigger("Death");
                        dead = true;
                        this.transform.position = new Vector3(10, 13, 0);
                    }

                }
                else
                {
                    if (!training)
                    {
                        health = 0;

                        animator.SetTrigger("Death");
                        dead = true;
                        this.transform.position = new Vector3(10, 13, 0);
                    }
                    

                }
            }

            enemyAttack = true;


        }
    }

    public void UpdateEnergy(float enAmount)
    {
        if(energy+enAmount <= MAX_ENERGY)
        {
            energy += enAmount;
        }
        else
        {
            energy = 300;
        }
    }

    public void FlipSprite(int state) //0 normal 1 inverse
    {
        if (state == 0 )
        {
            this.transform.rotation = Quaternion.Euler(0,0,0);
            lookForward = true;
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            lookForward = false;
        }
    }

    public void Attack1Sound()
    {
        GameUtils.PlaySound(attack1s, audioPlayer);

    }

    public void Attack2Sound()
    {
        GameUtils.PlaySound(attack2s, audioPlayer);

    }

    public void Attack3Sound()
    {
        GameUtils.PlaySound(attack3s, audioPlayer);

    }

    public void SpecialSound()
    {
        GameUtils.PlaySound(specials, audioPlayer);

    }

    public void JumpSound()
    {
        GameUtils.PlaySound(jumps, audioPlayer);

    }

    private void FixedUpdate()
    {
        //Move our character
      
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, attack);
            jump = false;
       
    }

    public Rigidbody2D Body
    {
        get
        {
            return this.myBody;
        }
    }

    public void GetKill()
    {
        timesKill++;
    }

    public void Restart()
    {
        this.transform.position = initialPosition;
        animator.SetTrigger("Spawn");
        dead = false;

        health = MAX_HEALTH;
        energy = 0;
        shield = MAX_SHIELD;
        horizontalMove = 0f;
        crouch = false;
        jump = false;
        attack = false;
        specialAttack = false;
        lookForward = true;
        enemyAttack = false;
        defending = false;

    }

    public void Heal()
    {
        health = MAX_HEALTH;

    }
       
    public float HealthPercent
    {
        get
        {

            return health / MAX_HEALTH;
        }
    }

    public float EnergyPercent
    {
        get
        {
            return energy / MAX_ENERGY;
        }
    }

    public float ShieldPercent
    {
        get
        {
            return shield / MAX_SHIELD;
        }
    }

    public void ForcedAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void ForcedSAttack()
    {
        if (energy >= 100)
        {
            specialAttack = true;
            animator.SetTrigger("Special_Attack");
            energy -= 100;
        }

    }

    }
