using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCleanup : MonoBehaviour
{
    public static ObjectCleanup instance;
    public List<GameObject> objectsToDestroy;
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

        AddObjects(PlayerBase.instance.gameObject);
        AddObjects(DialogueLoader.instance.gameObject);
        AddObjects(DialogueUI.instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObjects(GameObject objectToAdd)
    {
        objectsToDestroy.Add(objectToAdd);
    }

    public void CleanObjects()
    {
        foreach(GameObject objectToRemove in objectsToDestroy)
        {
            Destroy(objectToRemove);
        }
    }
}
