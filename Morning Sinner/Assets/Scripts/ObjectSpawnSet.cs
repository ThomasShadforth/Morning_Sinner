using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerBase.instance.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
