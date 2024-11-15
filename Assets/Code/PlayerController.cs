using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2 movementVector;
    private Rigidbody2D rb;
    const int speed = 4;
    public bool movementEnabled = true;
    int orb = 0;
    int animalpoints = 0;

    public DialogueRunner dialogueRunner;

    [SerializeField] Animator animator;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioClip orbCollected;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        dialogueRunner.onDialogueStart.AddListener(FreezeMovement);
        dialogueRunner.onDialogueComplete.AddListener(UnfreezeMovement);
        
    }
 

    // Update is called once per frame
    void Update()
    {
         if (movementEnabled) 
         {
            rb.velocity = new Vector2(speed * movementVector.x, speed * movementVector.y);

            movementVector.x = Input.GetAxisRaw("Horizontal");
            movementVector.y = Input.GetAxisRaw("Vertical");
        }
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

     
    }
    public void FreezeMovement()
    {
        rb.velocity = Vector2.zero;
        movementEnabled = false;
    }

    // Unfreeze player movement
    public void UnfreezeMovement()
    {
        movementEnabled = true;

    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orb"))
        {
            collision.gameObject.SetActive(false);
            orb++;
            SFXSource.PlayOneShot(orbCollected);
            Debug.Log("Orbs Obtained: " + orb);
        }
        
        
        if (collision.gameObject.CompareTag("Suit_Animal"))
        {
            animalpoints++;
            Debug.Log("Animal points: ");
        }

       
        if (collision.gameObject.CompareTag("Block_Collider_1"))
        {
            if (animalpoints >= 7)
                collision.gameObject.SetActive(false);
        }

    }

}
