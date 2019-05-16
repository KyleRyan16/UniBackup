using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    // Parameters
    Vector2 targetPos; // position character wants to move to when changing floors
    public float Yincriment; // how much above that target position to go to so that player doesn't clip into floor
    public float distance; // distance to move to new floor per tick, multiplied by time.deltatime so effectively meters per second.
    private bool movingToNewFloor; // while moving to new floor is true, prevents other actions getting in way
    private Vector2 jumpForce; // vertical force to apply when jump is pressed
    private int relativeInput; // 1 = character inputted up direction -1 = down direction, when wanting to change floors
    private float castDistance; // when checking for floor to change to, distance needs to be long enough to capture 2 floors at once.
    LayerMask castMask; // what layer to look for when casting, want to only hit floors currently
    private bool wantChangeFloor; // MovePlayer() receives input to change this, next line in update checks this for running ChangeFloor();
    private bool fastFalling;
    private bool jumping;
    private bool sliding;
    Vector3 castDirection;
    Vector2 slidePosition;

    // References
    Rigidbody2D rb;
    GameObject newFloor;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Initialise to be the same as current position, awaiting new value from input
        targetPos = transform.position;

        jumpForce = new Vector2(0, 600);
        castDistance = 9;
        castMask = LayerMask.GetMask("Floor");
        relativeInput = 1;
        castDirection = new Vector3(0, 1, 0);
        SwipeDetect.OnSwipe += Movement;

    }

    private void Movement(SwipeData data)
    {

        if (data.Direction == SwipeDirection.Up)
        {
            relativeInput = 1;

        }
        else if (data.Direction == SwipeDirection.Down)
        {
            relativeInput = -1;
        }
        else relativeInput = 0;


    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        if (wantChangeFloor)
        {
            if (!movingToNewFloor)
            {
               ChangeFloor(newFloor);
            }
            MoveToFloor();

        }

        //Vector3 cameraFollowTarget = new Vector3(Camera.main.transform.position.x, transform.position.y, -10f);

        //Camera.main.transform.position = Vector2.MoveTowards(Camera.main.transform.position, 
        //                                                        cameraFollowTarget, 
        //                                                        1f);

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }


    private void MovePlayer()
    {
        if (movingToNewFloor == false)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (CastForGround().transform.position.y >= transform.position.y - 1.06)
                {
                    jumping = false;
                    if (jumping == false)
                    {
                        Jump();
                    }
                    
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {

                Debug.Log("" + CastForGround().transform.position.y);

                if (CastForGround().transform.position.y >= transform.position.y - 1.06)
                {
                    Slide();
                    sliding = true;
                }
                else
                {
                    if (!fastFalling)
                    {
                        FastFall();
                        Debug.Log("fast falling");
                    }

                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                relativeInput = -1;
                RequestChangeFloor(relativeInput);

            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                relativeInput = 1;
                RequestChangeFloor(relativeInput);
            }

            rb.gravityScale = 1;
            GetComponent<Collider2D>().isTrigger = false;

        }
        else if (transform.position.y == targetPos.y)
        {
            movingToNewFloor = false;
            wantChangeFloor = false;
        }
    }

    private void RequestChangeFloor(int relativeInput)
    {
        newFloor = GetFloor(relativeInput);
        if (newFloor)
        {
            wantChangeFloor = true;
        }
    }

    private void Jump()
    {
        if(sliding)
        {
            Stand();
            sliding = false;
        }
        rb.AddForce(jumpForce);
        fastFalling = false;
        jumping = true;
        
    }

    private void FastFall()
    {
        rb.velocity = new Vector3(0,0,0);
        rb.AddForce(-jumpForce * 2);
        fastFalling = true;
        jumping = false;
    }

    private RaycastHit2D CastForGround()
    {
        return Physics2D.Raycast(
            transform.position, 
            -castDirection, 
            4f, 
            castMask);
    }

    private void Slide()
    {
        if (!sliding)
        {
            slidePosition = new Vector3(transform.position.x, CastForGround().transform.position.y, + 0.51f);
            Vector3 slideRotate = new Vector3(0, 0, 90);
            //Quaternion slideRotation;
            //slideRotation = new Quaternion()
            //transform.SetPositionAndRotation(slidePosition, )
            transform.Rotate(slideRotate);
            transform.position = Vector2.MoveTowards(transform.position, slidePosition, 1);
            sliding = true;
            StartCoroutine(WaitAndStopSlide(1f));
        }
    }
    private void Stand()
    {

        
        Vector3 slideRotate = new Vector3(0, 0, -90);
        slidePosition = new Vector2(transform.position.x, CastForGround().transform.position.y + 0.91f);
        transform.Rotate(slideRotate);
        transform.position = Vector2.MoveTowards(transform.position, slidePosition, 1);
        sliding = false;
    }

    // Lane-style changing between floors of buildings
    private GameObject GetFloor(int relativeInput)
    {
        GameObject hitObject = FloorCastHitResult(relativeInput).transform.gameObject;

        return hitObject;
     
    }


    private void ChangeFloor(GameObject newFloor)
    {

        rb.gravityScale = 0;
        GetComponent<Collider2D>().isTrigger = true;

        // reset velocity to provide consistant change to 
        rb.velocity = new Vector2(0, 0);

        if(sliding)
        {
            Stand();
        }
        // creates vector2 for MoveTowards function to utilise the floors position.
        targetPos = new Vector2(
            transform.position.x,
            newFloor.transform.position.y + Yincriment
            );

    }

    private void MoveToFloor()
    {
        // move to newFloor acquired through raycast.
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            distance * Time.deltaTime
            );

        movingToNewFloor = true;
    }



    RaycastHit2D FloorCastHitResult(int relativeInput)
    {
        Color color = new Color(1, 1, 0.5f);
        Vector3 castOffset = new Vector3(0, 1.05f * relativeInput, 0);
        Debug.DrawRay(transform.position, castDirection * relativeInput * castDistance, color);

        // if cast was up/positive relative input, cast for first floor hit, otherwise cast for floor past current floor on/ below player.
        if (relativeInput == 1)
        {
            return Physics2D.Raycast(transform.position + castOffset, castDirection * relativeInput, castDistance, castMask);
        }
        else
        {
            RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, castDirection * relativeInput, castDistance, castMask);


            return raycastHits[1];
        }

    }


    IEnumerator WaitAndStopSlide(float waitTime)
    {
        print("sliding");
        yield return new WaitForSeconds(waitTime);
        if (sliding)
        {
            Stand();
        }

    }
}
