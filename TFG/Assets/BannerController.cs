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


    public void ShowYouDie()
    {
        animator.SetTrigger("YouDie");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
