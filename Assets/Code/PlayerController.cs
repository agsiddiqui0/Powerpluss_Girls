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
        
    }

    void OnMove(InputValue value)
    {
        movementVector = value.Get<Vector2>();
    }

    /*void OnSprint(InputAction.CallbackContext context)
    {

    }*/

}
