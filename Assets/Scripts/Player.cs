using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private RectTransform cursorRectTransform;
    [SerializeField] public TextMeshProUGUI gameOverText;
    [SerializeField] public TextMeshProUGUI minutesSurvivedText;

    private bool isRunning = false;
    private bool isJumping = false;
    public bool aim = false;

    private PlayerInput playerInput;
    private Camera mainCam;
   

    int jumpCount = 0;
    private float minDistance = 0.1f;

    Vector2 inputVector;
    Vector2 mousePos;

   
    public Vector3 minBounds; // Minimum X, Y, Z coordinates for the playable area
    public Vector3 maxBounds;

    private float startTime;
    private bool isGameActive = true;

    private void Awake()
    {
        playerInput =  new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump_performed;
        playerInput.Player.Grab.started += Grab_started;
        playerInput.Player.Grab.canceled += Grab_canceled;
        
    }


    private void Grab_started(InputAction.CallbackContext obj)
    {
  
        aim = true;
    }

    private void Grab_canceled(InputAction.CallbackContext obj)
    {
      
        aim = false;
    }

   

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.performed && jumpCount < 2)
        {
            rb.AddForce(player.up * jumpSpeed, ForceMode.Impulse);

            jumpCount++;
            isJumping = true;

            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ
                             | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        mainCam = Camera.main;
        player.rotation = Quaternion.Euler(0, 0, 0);
        player.position = new Vector3(0f, 0f, 45f);
        startTime = Time.time;

    }

    void Update()
    {
        PlayerMove();
        ClampPlayerPosition();
        if (isGameActive)
        {
            UpdateMinutesSurvived();
        }
    }

    private void FixedUpdate()
    {
        PlayerLook();
    }

    void UpdateMinutesSurvived()
    {
        float elapsedTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (minutesSurvivedText != null)
        {
            minutesSurvivedText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    void ClampPlayerPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y, maxBounds.y);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBounds.z, maxBounds.z);

        transform.position = clampedPosition;
    }

   public void PlayerMove()
    {
        inputVector = playerInput.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveSpeedMultiplier = 1f;

        if (Vector3.Dot(moveDir, player.forward) < 0)
        {
            moveSpeed = 2f;
            moveSpeedMultiplier = 0.5f;
           

        }
        else
        {
            moveSpeed = 5f;
            moveSpeedMultiplier = 1f;
            

        }
        player.position += moveDir * moveSpeed * moveSpeedMultiplier * Time.deltaTime;


        isRunning = moveDir != Vector3.zero;
        
        
        if(jumpCount > 0)
        {
            isRunning = false;
        }

        
    }

    void PlayerLook()
    {
        mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {

            Vector3 targetPoint = hitInfo.point;
            Vector3 direction = (targetPoint - player.position).normalized;
          
            
            direction.y = 0f;
          

            if(direction != Vector3.zero && direction.magnitude > minDistance)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);



                // player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * 10f);
                gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, targetRotation * Quaternion.Euler(0, 180, 0), Time.deltaTime * 10f);

                player.rotation = Quaternion.Slerp(player.rotation, targetRotation , Time.deltaTime * 10f);
              
            }
           
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Elevation"))
        {
            
            jumpCount -= 2;
            if(jumpCount < 0)
            {
                jumpCount = 0;
            }
            isJumping = false;
            rb.constraints = RigidbodyConstraints.None;
        }

        if (collision.gameObject.tag.StartsWith("Enemy"))
        {
            isGameActive = false;
            gameOverText.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

   

  

}
