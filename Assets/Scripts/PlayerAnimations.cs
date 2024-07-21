using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Player player;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isRun", player.IsRunning());
        animator.SetBool("isJump", player.IsJumping());
     
    }
    void Start()
    {
        
    }

   
    void Update()
    {
        animator.SetBool("isRun", player.IsRunning());
        animator.SetBool("isJump", player.IsJumping());
      
    }
}
