using System;
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
    public ObjectHighlight highlightedObject;
    public bool hittingObject;

    //PuzzleObject grabbedObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 right = transform.TransformDirection(Vector3.right) * rayDistance * transform.localScale.x;
        Debug.DrawRay(playerHands.position, right, Color.red);
        checkForObjectGrab();
        checkForInteraction();

        
        checkForObjectUIHighlight();
        if (heldObject != null)
        {
            heldObject.transform.position = playerHands.transform.position;
        }
    }

    private void checkForObjectUIHighlight()
    {
        RaycastHit hit;
        Ray ray = new Ray(playerHands.position, Vector3.right * rayDistance * transform.localScale.x);
        GameObject hitObject;

        if (Physics.Raycast(ray, out hit))
        {
            hitObject = hit.transform.gameObject;
            
            if(hitObject.GetComponent<ObjectHighlight>() || hitObject.GetComponentInChildren<ObjectHighlight>())
            {
                if(highlightedObject != null)
                {
                    highlightedObject.isLitUp = false;
                    highlightedObject = null;
                }

                if (hitObject.GetComponent<ObjectHighlight>())
                {
                    hitObject.GetComponent<ObjectHighlight>().isLitUp = true;
                    highlightedObject = hitObject.GetComponent<ObjectHighlight>();
                } else if (hitObject.GetComponentInChildren<ObjectHighlight>())
                {
                    hitObject.GetComponentInChildren<ObjectHighlight>().isLitUp = true;
                    highlightedObject = hitObject.GetComponentInChildren<ObjectHighlight>();
                }

                UIGrabText.instance.UIText.gameObject.SetActive(true);
                setUIText(highlightedObject);
            }
            else
            {
                if(highlightedObject != null)
                {
                    highlightedObject.isLitUp = false;
                    highlightedObject = null;
                    hittingObject = false;
                    UIGrabText.instance.UIText.gameObject.SetActive(false);
                }
                
            }
        }
        

    }

    void setUIText(ObjectHighlight objectToHighlight)
    {
        if (objectToHighlight.type == objectType.Grabbable)
        {
            UIGrabText.instance.UIText.text = "Hold left mouse to grab object!";
        }
        else
        {
            UIGrabText.instance.UIText.text = "Right click to investigate!";
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
            Ray ray = new Ray(playerHands.position, Vector3.right * rayDistance * transform.localScale.x);
            if(Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if(hitObject.layer == LayerMask.NameToLayer("PuzzleObjects") && !isGrabbing)
                {
                    

                    Destroy(hitObject.GetComponent<Rigidbody>());
                    heldObject = hitObject;

                    if (transform.position.x >  heldObject.transform.position.x)
                    {
                        heldObject.transform.localScale = setObjectScale(-1f, heldObject.transform.localScale);
                    }
                    else
                    {
                        heldObject.transform.localScale = setObjectScale(1f, heldObject.transform.localScale);
                    }
                    heldObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    heldObject.transform.parent = playerHands.transform;
                    heldObject.GetComponent<GrabCube>().pickupObject();
                    isGrabbing = true;
                    PlayerBase.instance.grabbing = true;
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
                heldObject.AddComponent<Rigidbody>();
                heldObject.GetComponent<Rigidbody>().AddForce(new Vector3(2 * heldObject.transform.localScale.x, 0, 0), ForceMode.Impulse);
                heldObject = null;
                isGrabbing = false;
                PlayerBase.instance.grabbing = false;
            }
        }
    }

    void checkForInteraction()
    {
        if (!DialogueManager.instance.dialogueInProg)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                RaycastHit hit;
                Ray ray = new Ray(playerHands.position, Vector3.right * rayDistance * transform.localScale.x);
                if (Physics.Raycast(ray, out hit))
                {
                    
                    GameObject hitObject = hit.transform.gameObject;
                    if (hitObject.layer == LayerMask.NameToLayer("Interactables"))
                    {

                        //DialogueManager.instance.testDialogueBox.gameObject.SetActive(!DialogueManager.instance.testDialogueBox.activeInHierarchy);
                        TriggerDialogue trigger = hitObject.gameObject.GetComponent<TriggerDialogue>();
                        trigger.DialogueTrig();
                    }
                }
            }
        }
    }

    Vector3 setObjectScale(float objectScalarX, Vector3 objectTransformScale)
    {
        Vector3 scalar = objectTransformScale;
        scalar.x = 1 * objectScalarX;//scalar.x * objectScalarX;
        objectTransformScale = scalar;
        return scalar;
    }
}
