using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum objectType
{
    Dialogue,
    Grabbable
}

public class ObjectHighlight : MonoBehaviour
{
    public bool isLitUp;
    public bool hasSet = true;
    public GameObject interactUI;

    public Material[] materials;
    MeshRenderer objRenderer;

    public objectType type;

    // Start is called before the first frame update
    void Start()
    {
        objRenderer = GetComponentInChildren<MeshRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isLitUp && !hasSet)
        {
            objRenderer.material = materials[1];
            
            
            hasSet = true;
        }
        else if(!isLitUp && hasSet)
        {
            objRenderer.material = materials[0];
            //interactUI.SetActive(false);
            hasSet = false;
        }
    }

    
}
