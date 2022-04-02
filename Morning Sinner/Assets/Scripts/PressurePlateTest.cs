using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTest : MonoBehaviour
{
    [SerializeField] PuzzleDoor attachedDoor;
    public List<GameObject> objectsOnPlate = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (objectsOnPlate.Count == 0)
        {
            attachedDoor.RotateDoor(true);
        }
        

        if(objectsOnPlate.Count > 0)
        {
            foreach(GameObject plateObject in objectsOnPlate)
            {
                if(other.gameObject != plateObject)
                {
                    objectsOnPlate.Add(other.gameObject);
                }
            }
        }
        else
        {
            objectsOnPlate.Add(other.gameObject);
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        objectsOnPlate.Remove(other.gameObject);
        if (objectsOnPlate.Count == 0)
        {
            attachedDoor.RotateDoor(false);
        }
    }
}
