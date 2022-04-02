using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCube : MonoBehaviour
{
    public PressurePlateTest weighedPlate;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pickupObject()
    {
        if(weighedPlate != null)
        {
            foreach(GameObject plateObject in weighedPlate.objectsOnPlate)
            {
                if(plateObject == this.gameObject)
                {
                    weighedPlate.objectsOnPlate.Remove(plateObject);
                }
            }
        }
    }

    public void releaseObject()
    {
        
    }
}
