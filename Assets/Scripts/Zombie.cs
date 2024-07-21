using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private bool isAlive = true;
    Rigidbody rb;
    ObjectPooler op;


    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        op = ObjectPooler.instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool IsAlive()
    {
        return isAlive;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Elevation")
        {
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);
        }

        
    }
}
