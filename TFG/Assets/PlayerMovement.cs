using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool crouch = false;
    bool jump = false;
    bool atack = false;

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Movement", horizontalMove);
        if (Input.GetButtonDown("Jump"))
        {
                jump = true;
                animator.SetBool("OnAir", true);

            
        }

        if (Input.GetButtonDown("Atack"))
        {

            animator.SetTrigger("Atack");

  
        }
        
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            animator.SetBool("IsCrouching",true);
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            animator.SetBool("IsCrouching", false);

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

    private void FixedUpdate()
    {
        //Move our character

        controller.Move(horizontalMove * Time.fixedDeltaTime,crouch,jump,atack);
        jump = false;
      
    }
}
