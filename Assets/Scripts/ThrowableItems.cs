using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItems : MonoBehaviour
{
    public bool canKill = false;
   
    int hitCount = 0;
   
    ObjectPooler op;
    ObjectAttractor oa;

    private void Start()
    {
        op = ObjectPooler.instance;
        oa = FindObjectOfType<ObjectAttractor>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Ground")
        {
            canKill = false;
        }

        if (oa.IsThrown())
        {
            canKill = true;
            if (other.gameObject.tag.StartsWith("Enemy"))
            {

                {
                    hitCount++;
                    if (hitCount >= 3)
                    {
                        
                        op.DeactivateGameObject(other.gameObject.tag, other.gameObject);
                        hitCount = 0;
                    }

                }


            }
        }

         
        
    }


}
