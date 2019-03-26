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

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool crouch = false;
    bool jump = false;
    bool atack = false;
    bool lookFoward = true;
    bool defending = false;
    bool dead = false;

    Vector3 initialPosition;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialPosition = this.transform.position;
    }

    public void UpdateHumanInput ()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;   
        animator.SetFloat("Movement", horizontalMove);

        if ((horizontalMove > 0.1 && lookFoward == true) || (horizontalMove < -0.1 && lookFoward == false))
        {
            
            
            animator.SetBool("Walk_Forward", true);
        }
        else
        {
            animator.SetBool("Walk_Forward", false);

        }
        if ((horizontalMove < -0.1 && lookFoward == true) || (horizontalMove > 0.1 && lookFoward == false))
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
        if (Input.GetButtonDown("Atack"))
        {
            animator.SetTrigger("Atack");
        }
        if (Input.GetButtonDown("SpecialAtack"))
        {
            if (energy >= 100)
            {
                animator.SetTrigger("Special_Atack");
                energy -= 100;
            }
        }

    } 

    public void UpdateHuman2Input()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal2") * runSpeed;
        animator.SetFloat("Movement", horizontalMove);

        if ((horizontalMove < -0.1 && lookFoward == true) || (horizontalMove > 0.1 && lookFoward == false))
        {
            animator.SetBool("Walk_Forward", true);
        }
        else
        {
            animator.SetBool("Walk_Forward", false);

        }
        if ((horizontalMove > 0.1 && lookFoward == true) || (horizontalMove < -0.1 && lookFoward == false))
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
            animator.SetBool("OnAir", true);
        }

        if (Input.GetButtonDown("Crouch2"))
        {
            crouch = true;
            animator.SetBool("Is_Crouching", true);
            if (shield > 0)
            {
                defending = true;
                animator.SetBool("Defending", true);

            }
        }
        else if (Input.GetButtonUp("Crouch2"))
        {
            crouch = false;
            animator.SetBool("Is_Crouching", false);
            defending = false;
            animator.SetBool("Defending", false);

        }
        if (Input.GetButtonDown("Atack2"))
        {
            animator.SetTrigger("Atack");
        }
        if (Input.GetButtonDown("SpecialAtack2"))
        {
            if (energy >= 100){
                animator.SetTrigger("Special_Atack");
                energy -= 100;
            }
        }

    }

    void Update()
    {
        animator.SetFloat("Health", HealthPercent);

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
    }

    public void StartAtack()
    {
        atack = true;
        

    }
    public void EndAtack()
    {
        atack = false;
    }

    public void ThrowSphere()
    {
        Vector3 SphereInitialPosition;

        if (spriteRendered.flipX == false)
        {
            SphereInitialPosition = new Vector3(this.transform.position.x -1, this.transform.position.y, 0);
        }
        else
        {
            SphereInitialPosition = new Vector3(this.transform.position.x , this.transform.position.y, 0);
   
        }
        EnergySphere clone = EnergySphere.Instantiate(
            sphere,
            SphereInitialPosition,
            Quaternion.Euler(0,0,0));
        clone.caster = this;
    }

    public void DamageReceived(float damage)
    {
        if (dead == false)
        {
            this.UpdateEnergy(35);
            if (defending == true && shield > 0)
            {
                if (shield >= damage)
                {
                    shield -= damage;
                    if (shield == 0)
                    {
                        animator.SetBool("Defending", false);
                    }
                }
                else
                {
                    health = health - (damage - shield);
                    shield = 0;
                    animator.SetBool("Defending", false);

                }
            }
            else
            {
                if (health >= damage)
                {
                    health -= damage;
                    if (health == 0)
                    {
                        //oponent.GetKill();
                        animator.SetTrigger("Death");
                        dead = true;
                        this.transform.position = new Vector3(10, 13, 0);
                    }

                }
                else
                {
                    health = 0;
                   // oponent.GetKill();
                    animator.SetTrigger("Death");
                    dead = true;
                    this.transform.position = new Vector3(10, 13, 0);

                }
            }


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

    public void FlipSprite(int state) //0 normal 1 inverse
    {
        if (state == 0 )
        {
            this.transform.rotation = Quaternion.Euler(0,0,0);
            lookFoward = true;
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            lookFoward = false;
        }
    }

    private void FixedUpdate()
    {
        //Move our character
        if (player == PlayerType.HUMAN)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, atack);
            jump = false;
        }

        if (player == PlayerType.HUMAN2)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, atack);
            jump = false;
        }
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
        health = MAX_HEALTH;
        energy = 0;
        shield = MAX_SHIELD;
        horizontalMove = 0f;
        crouch = false;
        jump = false;
        atack = false;
        lookFoward = true;
        defending = false;
        dead = false;
        animator.SetTrigger("Spawn");

    }
}
