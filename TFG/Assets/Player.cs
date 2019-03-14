using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerType
    {

        HUMAN, AI

    };


    public static float MAX_HEALTH = 100f;

    public CharacterController2D controller;
    public Animator animator;
    public float health = MAX_HEALTH;
    public string playerName;
    public Player oponent;
    public PlayerType player;

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
            animator.SetTrigger("Special_Atack");
        }

    }
    void Update()
    {

        animator.SetFloat("Health", healthPercent);

        if (oponent != null)
        {
            animator.SetFloat("Oponent_Health", oponent.healthPercent);
        } else
        {
            animator.SetFloat("Oponent_Health", 1);
        }


     if (player == PlayerType.HUMAN)
        {
            UpdateHumanInput();
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
    public float healthPercent
    {
        get
        {
            return health / MAX_HEALTH;
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
    }
    public Rigidbody2D body
    {
        get
        {
            return this.myBody;
        }
    }
}
