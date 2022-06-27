using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCam : MonoBehaviour
{
    public static PlayerCam instance;
    CinemachineVirtualCamera camControl;


    // Start is called before the first frame update
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

        camControl = GetComponent<CinemachineVirtualCamera>();
        camControl.LookAt = FindObjectOfType<PlayerBase>().transform;
        camControl.Follow = FindObjectOfType<PlayerBase>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
