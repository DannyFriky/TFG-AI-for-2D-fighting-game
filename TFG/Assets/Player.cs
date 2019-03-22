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

    public CharacterController2D controller;
    public Animator animator;
    public float health = MAX_HEALTH;
    public float energy = 0;
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

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void UpdateHumanInput ()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;   
        animator.SetFloat("Movement", horizontalMove);

        if (horizontalMove > 0.1)
        {
            animator.SetBool("Walk_Forward", true);
        }
        else
        {
            animator.SetBool("Walk_Forward", false);

        }
        if (horizontalMove < -0.1)
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
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            animator.SetBool("Is_Crouching", false);
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

        if (horizontalMove < -0.1)
        {
            animator.SetBool("Walk_Forward", true);
        }
        else
        {
            animator.SetBool("Walk_Forward", false);

        }
        if (horizontalMove > 0.1)
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
        }
        else if (Input.GetButtonUp("Crouch2"))
        {
            crouch = false;
            animator.SetBool("Is_Crouching", false);
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


     if (player == PlayerType.HUMAN)
        {
            UpdateHumanInput();
        }
        if (player == PlayerType.HUMAN2)
        {
            UpdateHuman2Input();
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
        this.UpdateEnergy(35);
        if (health >= damage)
        {
            health -= damage;
            if (health == 0) {
                animator.SetTrigger("Death");
            }

        }
        else
        {
            health = 0;
            animator.SetTrigger("Death");
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

    public void FlipSprite(int state) //0 normal 1 inverse
    {
        if (state == 0 )
        {
            this.transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);

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
}
