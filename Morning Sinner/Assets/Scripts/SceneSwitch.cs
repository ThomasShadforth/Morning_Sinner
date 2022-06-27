using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    bool isMovingScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBase>() && !isMovingScene)
        {
            isMovingScene = true;
            StartCoroutine(MoveSceneCo());
        }
    }

    IEnumerator MoveSceneCo()
    {
        UIFade.instance.fadeToBlack();
        yield return new WaitForSeconds(1f);
        UIFade.instance.fadeFromBlack();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
