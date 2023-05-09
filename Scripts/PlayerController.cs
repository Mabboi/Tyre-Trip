using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;
    private int desiredLane = 1;// 0:left 1:middle 2:right
    public float laneDistance = 4;// distance between two lanes

    public float jumpForce;
    public float Gravity = -20;

    public Animator animator;

    public float wheelNormalPositionNumb = 1f;
    public GameObject slideDownCounter;


    void Start()
    {
        //gather inputs on which lane we should be
        controller = GetComponent<CharacterController>();
        controller.height = 2f; 
    }

    // Update is called once per frame
    public void Update()
    {
        if (slideDownCounter.activeInHierarchy)
        {
            wheelNormalPositionNumb += Time.deltaTime;
        }
        direction.z = forwardSpeed;
        
        if (controller.isGrounded) 
        {
                
            if (SwipeManager.swipeUp)
            {
                animator.SetBool("isGrounded", true);
                Jump();
            }
            else 
            {
                animator.SetBool("isGrounded", false);
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }
        
        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        if (SwipeManager.swipeDown)
        {
            wheelNormalPositionNumb -= 1;
            slideDownCounter.SetActive(true);
            animator.SetBool("isSlide", true);
            controller.height = 1.8f;   
        }
        else 
        {
            animator.SetBool("isSlide", false);
            if (slideDownCounter.activeInHierarchy && wheelNormalPositionNumb >= 1.5f)
            {
                wheelNormalPositionNumb = 1;
                controller.height = 2f;
                slideDownCounter.SetActive(false);
            }

        }
        //calculate where we should be in the future

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0) 
        {
            targetPosition += Vector3.left * laneDistance;
        }else if(desiredLane == 2) 
        {
            targetPosition += Vector3.right * laneDistance;
        }

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

    }

    private void FixedUpdate() 
    {
        if (!PlayerManager.isGameStarted)
            return;
        if(forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;
        
        animator.SetBool("isGameStarted",true);
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump() 
    {
        direction.y = jumpForce;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        if(hit.transform.tag == "Obstacle") 
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }
}
