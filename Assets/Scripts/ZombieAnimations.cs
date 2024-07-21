using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimations : MonoBehaviour
{
   
    [SerializeField] private Animator e_animator;

    private void Awake()
    {
        
    }
    void Start()
    {
        e_animator = GetComponent<Animator>();
        e_animator.SetBool("isAlive", true);
    }

   
    void Update()
    {
        e_animator.SetBool("isAlive", true);
    
    }
}
