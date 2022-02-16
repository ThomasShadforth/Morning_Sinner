using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public float rayDistance;
    public bool isGrabbing;
    public float grabbedObjWeight;
    [SerializeField] Transform playerHands;
    public LayerMask ObjectToGrab;
    GameObject heldObject;
    
    //PuzzleObject grabbedObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 right = transform.TransformDirection(Vector3.right) * rayDistance * transform.localScale.x;
        Debug.DrawRay(transform.position, right, Color.red);
        checkForObjectGrab();
        checkForInteraction();
        if (heldObject != null)
        {
            heldObject.transform.position = playerHands.transform.position;
        }
    }

    void checkForObjectGrab()
    {
        if (Input.GetButton("Fire1"))
        {
            /*
            //fire raycast, check for object
            RaycastHit hit = Physics.Raycast(transform.position, Vector3.right * transform.localScale.x, rayDistance, ObjectToGrab);
            //if object is detected, then set 
            if (objectDetect)
            {
                //GameObject obj = Physics.Raycast(transform.position, Vector3.right * transform.localScale.x, rayDistance, ObjectToGrab);
                //grabbedObj = targetObj;
                grabbedObjWeight = 50;
                isGrabbing = true;
            }*/

            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.right * rayDistance * transform.localScale.x);
            if(Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if(hitObject.layer == LayerMask.NameToLayer("PuzzleObjects"))
                {
                    heldObject = hitObject;
                    heldObject.transform.parent = playerHands.transform;
                    isGrabbing = true;
                    grabbedObjWeight = 50;
                }
                
            }
        }

        if (isGrabbing)
        {
            if (Input.GetButtonUp("Fire1") /*|| stamina <= 0*/)
            {
                //release the object, set isGrabbing to false
                heldObject.transform.parent = null;
                heldObject = null;
                isGrabbing = false;
            }
        }
    }

    void checkForInteraction()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.right * rayDistance * transform.localScale.x);
            if(Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if(hitObject.layer == LayerMask.NameToLayer("Interactables"))
                {
                    //DialogueManager.instance.testDialogueBox.gameObject.SetActive(!DialogueManager.instance.testDialogueBox.activeInHierarchy);
                    DialogueTrigger trigger = hitObject.gameObject.GetComponent<DialogueTrigger>();
                    trigger.triggerDialogue();
                }
            }
        }
    }
}
