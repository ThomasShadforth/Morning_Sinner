using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject LogoObject;
    [SerializeField] GameObject MenuTitle;
    [SerializeField] GameObject MenuContainer;

    
    public static MainMenu instance;

    bool isMenuOpen;

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
        animator = GetComponent<Animator>();
        //LogoObject.SetActive(true);
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!isMenuOpen)
            {
                isMenuOpen = true;
                animator.SetBool("isMenuOpening", true);
            }
        }
    }

    public void setObjectActive()
    {
        LogoObject.SetActive(false);
        MenuTitle.SetActive(true);
        UIFade.instance.fadeFromBlack();
        animator.Play("MenuTitleFadeIn");
    }

    public void openMenuScreen()
    {
        MenuContainer.SetActive(true);
        animator.Play("MenuFadeIn");
        
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }
}
