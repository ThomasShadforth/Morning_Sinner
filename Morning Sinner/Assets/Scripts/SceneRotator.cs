using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRotator : MonoBehaviour
{
    public Vector3 rotation;
    public static SceneRotator instance;
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        PlayerBase.instance.transform.localEulerAngles = rotation;
        PlayerCam.instance.transform.localEulerAngles = new Vector3(PlayerCam.instance.transform.localEulerAngles.x, rotation.y, rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
