using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AI_States { 
    idle,
    patrol,
    chase
}
public class BasicAI : MonoBehaviour
{
    public Transform playerTarget;
    public float detectDistance;
    public float moveSpeed;
    public float captureDistance;
    public AI_States state;

    AI_States defaultState;

    [SerializeField] Transform[] walkingPositions;
    public float minimumPositionDistance;
    bool isCatching;
    int patrolIndex;
    public float waitTime;
    float waitTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerTarget = PlayerBase.instance.transform;
        defaultState = state;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCatching)
        {
            return;
        }

        DistanceCheck();

        if(state == AI_States.chase)
        {
            ChaseTarget();
        }

        if(state == AI_States.patrol)
        {
            Patrol();
        }
    }

    public void DistanceCheck()
    {
        if(Vector3.Distance(transform.position, playerTarget.position) <= detectDistance)
        {
            state = AI_States.chase;
        }

        if(Vector3.Distance(transform.position, playerTarget.position) > detectDistance)
        {
            state = defaultState;
        }
    }

    public void Patrol()
    {
        if(waitTimer <= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, walkingPositions[patrolIndex].position, moveSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, walkingPositions[patrolIndex].position) <= minimumPositionDistance)
            {
                waitTimer = waitTime;

                patrolIndex++;

                if(patrolIndex > walkingPositions.Length)
                {
                    patrolIndex = 0;
                }
            }
        }
        else
        {
            waitTimer -= Time.deltaTime;
        }
    }

    public void ChaseTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, playerTarget.position) <= captureDistance)
        {
            if (!isCatching)
            {
                isCatching = true;
            }
            DemoEnd();
        }
    }

    void DemoEnd()
    {
        StartCoroutine(DemoEndCo());
    }

    IEnumerator DemoEndCo()
    {
        UIFade.instance.fadeToBlack();
        yield return new WaitForSeconds(1f);
        ObjectCleanup.instance.CleanObjects();
        SceneManager.LoadScene("Demo_End");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectDistance);
    }
}
