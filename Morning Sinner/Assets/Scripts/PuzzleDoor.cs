using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour
{
    public Vector3 startRotation;
    public Vector3 endRotation;
    public float timeToRotate;
    float percentageRot;
    bool rotate;
 
    // Start is called before the first frame update
    void Start()
    {
        //startRotation = transform.localEulerAngles;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateDoor(bool willOpen)
    {
        rotate = willOpen;   
        StartCoroutine(rotateDoor(willOpen));


    }

    public IEnumerator rotateDoor(bool isOpening)
    {
        float timePercentage;
        timePercentage = percentageRot;

        if (isOpening)
        {
            

            while(timePercentage < 1)
            {
                timePercentage += Time.deltaTime / timeToRotate;
                percentageRot = timePercentage;
                transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, timePercentage);
                if (!rotate)
                {
                    break;
                }
                yield return null;
            }
        }
        else
        {
            

            while(timePercentage > 0)
            {
                timePercentage -= Time.deltaTime / timeToRotate;
                percentageRot = timePercentage;
                transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, timePercentage);
                if (rotate)
                {
                    break;
                }
                yield return null;
            }
        }
    }
}
