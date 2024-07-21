using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairPlacement : MonoBehaviour
{


    private void Awake()
    {
        
     
    }

    void Update()
    {
        transform.position = Input.mousePosition;

    }
}
