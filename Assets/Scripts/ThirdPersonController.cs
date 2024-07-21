using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private Transform crosshair;
    private Player playerInputs;



    private void Awake()
    {
        playerInputs = GetComponent<Player>();
        aimVirtualCamera.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (playerInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            crosshair.gameObject.SetActive(true);
            Cursor.visible = false;
                
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            crosshair.gameObject.SetActive(false);
            Cursor.visible = true;
        }
    }
}
