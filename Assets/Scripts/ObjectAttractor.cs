using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectAttractor : MonoBehaviour
{
    [SerializeField] private Transform gun;
    [SerializeField] private float throwSpeed = 20f;
    [SerializeField] private float gravityDelay = 1f;

    public int maxAttachedItems = 5; 
    private float itemSpacing = 0.5f;


    Camera mainCamera;
    

    List<GameObject> attachedItems;

    public bool isThrown = false;


    private void Awake()
    {
        attachedItems = new List<GameObject>();
        attachedItems.Clear();
    }
    void Start()
    {
        
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleLeftClick();
        HandleRightClick();
        UpdateAttachedItems();
    }

 
    void HandleRightClick()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseInput = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mouseInput);
           if( Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                GameObject hitObject = hitInfo.collider.gameObject;
                if(hitObject.CompareTag("Attractable") && attachedItems.Count < maxAttachedItems)
                {
                    attachedItems.Add(hitObject);
                    hitObject.GetComponent<Collider>().enabled = false;
                    hitObject.GetComponent<Rigidbody>().useGravity = false;
                    
                  
                     
                }
            }
        }
        
    }

    void HandleLeftClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && attachedItems.Count > 0)
        {
            ReleaseItem();
            isThrown = true;
        }
    }

    public bool IsThrown()
    {
        return isThrown;
    }
    void ReleaseItem()
    {
           
            GameObject removableItem = attachedItems[attachedItems.Count - 1];
            attachedItems.RemoveAt(attachedItems.Count - 1);

            Collider itemCollider = removableItem.GetComponent<Collider>();
            if (itemCollider != null)
            {
               itemCollider.enabled = true;
            }

            Rigidbody itemRigidbody = removableItem.GetComponent<Rigidbody>();
            if (itemRigidbody != null)
            {
              
               // itemRigidbody.GetComponent<Rigidbody>().useGravity = true;
                itemRigidbody.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                itemRigidbody.AddForce(transform.forward * throwSpeed, ForceMode.Impulse);
                StartCoroutine(ReEnableGravity(itemRigidbody, gravityDelay));

        }
        
    }

    IEnumerator ReEnableGravity(Rigidbody rb, float delay)
    {
      yield return new WaitForSeconds(delay);
        rb.useGravity = true;
    }

    void UpdateAttachedItems()
    {
        for(int i = 0; i < attachedItems.Count; i++)
        {
            GameObject item = attachedItems[i];
            Vector3 targetPosition = gun.position + transform.forward * itemSpacing * (i + 1);
         
                item.transform.position = Vector3.Lerp(item.transform.position, targetPosition, Time.deltaTime * 10);
             
                 item.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
              // Item has reached the target position, freeze constraints
                Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
                if (itemRigidbody != null)
                {
                  itemRigidbody.constraints = RigidbodyConstraints.FreezeAll; // Freeze position and rotation
                }
            

        }
    }

}
