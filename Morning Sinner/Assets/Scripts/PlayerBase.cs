using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("Movement Values")]
    public float moveSpeed;
    float defMoveSpeed;

    [Header("Ground Detection")]
    public float feetRadius;
    [SerializeField] Transform feetPos;
    public LayerMask whatIsGround;

    [Header("Jump - Count, timer, etc")]
    public int extraJumps;
    int jumpCount;
    public float jumpTime;
    [SerializeField] float jumpTimer;
    public float jumpForce;
    bool isGrounded, isJumping;
    bool isFacingRight;

    public Vector3 move;
    int moveVal;

    Rigidbody rb;
    Animator animator;

    //Instance variable
    public static PlayerBase instance;
    Vector3 positiveScale, negativeScale, lastMoveDirection;

    PlayerGrab grab;

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
            DontDestroyOnLoad(gameObject);
        }

        rb = GetComponent<Rigidbody>();
        jumpCount = extraJumps;
        moveVal = 1;
        grab = GetComponent<PlayerGrab>();
        defMoveSpeed = moveSpeed;
        negativeScale = transform.localScale * -1;
        positiveScale = transform.localScale;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        checkForJump();
        checkForGrab();
        checkDirection();
        rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
        Animate();

    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(feetPos.position, feetRadius, whatIsGround);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        if((moveX == 0 && moveZ == 0) && move.x != 0 || move.z != 0)
        {
            lastMoveDirection = move;
        }



        move = new Vector3(moveX, 0, moveZ).normalized;

        
    }

    #region Movement Methods

    void checkForJump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            //rigidbody velo
            rb.velocity = Vector3.up * jumpForce;
            jumpCount--;
            jumpTimer = jumpTime;
            isJumping = true;
        }

        if(Input.GetButtonDown("Jump") && isGrounded && jumpCount == 0)
        {
            rb.velocity = Vector3.up * jumpForce;
            jumpTimer = jumpTime;
            isJumping = true;
        }

        if(Input.GetButton("Jump") && isJumping)
        {
            if(jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;
                rb.velocity = Vector3.up * jumpForce;
            }
            else
            {
                Debug.Log("JUMP GONE");
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        if (isGrounded)
        {
            jumpCount = extraJumps;
        }
    }

    void checkDirection()
    {
        if (!grab.isGrabbing)
        {
            if (move.x > 0)
            {
                moveVal = 1;
            }
            else if (move.x < 0)
            {
                moveVal = -1;
            }

            if (moveVal > 0)
            {
                Vector3 scalar = transform.localScale;
                scalar.x = positiveScale.x;
                transform.localScale = scalar;
            }
            else
            {
                Vector3 scalar = transform.localScale;
                scalar.x = negativeScale.x;
                transform.localScale = scalar;
            }
        }
    }


    #endregion

    #region Player Actions
    void checkForGrab()
    {
        if (grab.isGrabbing)
        {
            //Reduce the player's speed depending on the object weight while they're pushing/pulling
            //NOTE: WILL NEED TO RUN CHECK DEPENDING ON WEIGHT OF OBJECT/Value of moveSpeed
            moveSpeed = (1 / grab.grabbedObjWeight) * 50;

            if(moveSpeed > 1)
            {
                moveSpeed = 1;
            }
        }
        else
        {
            //set moveSpeed back to normal
            moveSpeed = defMoveSpeed;
        }
    }

    #endregion

    #region Utility Methods
    private void Animate()
    {
        animator.SetFloat("Anim_Move_X", move.x);
        animator.SetFloat("Anim_Move_Y", move.z);
        animator.SetFloat("Anim_Move_Magnitude", move.magnitude);
        animator.SetFloat("Anim_Last_Move_X", lastMoveDirection.x);
        animator.SetFloat("Anim_Last_Move_Y", lastMoveDirection.z);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(feetPos.position, feetRadius);
    }

    #endregion
}
