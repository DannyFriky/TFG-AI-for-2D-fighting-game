using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerController : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }


    public void ShowYouDied()
    {
        animator.SetTrigger("YouDied");
    }

    public void ShowYouWin()
    {
        animator.SetTrigger("YouWin");
    }

    public void ShowP1Win()
    {
        animator.SetTrigger("P1Win");
    }

    public void ShowP2Win()
    {
        animator.SetTrigger("P2Win");
    }
}
