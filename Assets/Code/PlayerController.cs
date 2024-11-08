using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2 movementVector;
    private Rigidbody2D rb;
    int speed = 4;

    [SerializeField] Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(speed * movementVector.x, speed * movementVector.y);
        Debug.Log("speed is " + speed);

        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");



    }

    void OnMove(InputValue value)
    {
        movementVector = value.Get<Vector2>();
        animator.SetBool("Walk_Side", !Mathf.Approximately(movementVector.x, 0));
        animator.SetBool("Walk_Front", !Mathf.Approximately(movementVector.y, 0));
        animator.SetBool("Walk_Back", !Mathf.Approximately(movementVector.y, 0));

        if(!Mathf.Approximately(movementVector.x, 0))
        {
            sr.flipX = movementVector.x < 0;
        }

        if (movementVector.y < 0)
        {
            animator.SetBool("Walk_Back", false);
            animator.SetBool("Walk_Front", true);
        }
        else if (movementVector.y > 0)
        {
            animator.SetBool("Walk_Back", true);
            animator.SetBool("Walk_Front", false);
        }
        else
        {
            animator.SetBool("Walk_Back", false);
            animator.SetBool("Walk_Front", false);
        }

     



        /*
           if (movementVector.y < 0)
           {
               animator.Walk_Front = true;
           }
        */
    }

    void OnSprintPress (InputValue value)
    {
        speed = 7;
    }

    void OnSprintRelease ( )
    {
        speed = 4;
    }

    

}
