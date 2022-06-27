using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGrabText : MonoBehaviour
{
    public static UIGrabText instance;
    public TextMeshProUGUI UIText;
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

        UIText = GetComponentInChildren<TextMeshProUGUI>();
        UIText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
