using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIFade : MonoBehaviour
{
    public bool shouldFadeToBlack;
    public bool shouldFadeFromBlack;

    public Image UIFadeImage;

    public static UIFade instance;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeFromBlack)
        {
            UIFadeImage.color = new Color(UIFadeImage.color.r, UIFadeImage.color.g, UIFadeImage.color.b, Mathf.MoveTowards(UIFadeImage.color.a, 0f, 1 * Time.deltaTime));

            if(UIFadeImage.color.a == 0)
            {
                UIFadeImage.gameObject.SetActive(false);
                shouldFadeFromBlack = false;
            }
        }

        if (shouldFadeToBlack)
        {
            UIFadeImage.gameObject.SetActive(true);
            UIFadeImage.color = new Color(UIFadeImage.color.r, UIFadeImage.color.g, UIFadeImage.color.b, Mathf.MoveTowards(UIFadeImage.color.a, 1f, 1 * Time.deltaTime));

            if(UIFadeImage.color.a == 1)
            {
                shouldFadeToBlack = false;
            }
        }
    }

    public void fadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void fadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }
}
