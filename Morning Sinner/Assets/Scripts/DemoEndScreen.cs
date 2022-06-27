using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoEndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadMainMenu", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
