using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2 movementVector;
    private Rigidbody2D rb;
    public float baseSpeed = 4f;
    public float sprintMultiplier = 5f;

    public bool movementEnabled = true;
    int orb = 0;
    public bool curatetalk = false;
    private GameObject tele;
    private GameObject tele1;
    private GameObject tele2;
    public int animalpoints = 0;
    public int bubbaPoints = 0;
    private GameObject Effargia;
    private GameObject tellON;
    private GameObject currentTeleporter;
    private bool isTeleporting = false; // Prevents immediate re-teleportation

    public DialogueRunner dialogueRunner;

    [SerializeField] AudioMixer mixer;

    [SerializeField] Animator animator;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioClip orbCollected;
    [SerializeField] Transform destination;
    
    [SerializeField] private float teleportCooldown = 0.5f; // Cooldown time in seconds

    public GameObject projectilePrefab;
    public GameObject lightningPrefab;
    private Vector2 lastMoveDirection = Vector2.right;
    public float projectileSpeed = 100f;

    public void OnMusicVOlumeChange(float value)
    {
        mixer.SetFloat("Master", value);
    }
    public void OnSFXVolumeChange(float value)
    {
        mixer.SetFloat("underground", value);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        dialogueRunner.onDialogueStart.AddListener(FreezeMovement);
        dialogueRunner.onDialogueComplete.AddListener(UnfreezeMovement);

        if (SceneManager.GetActiveScene().name == "Defne")
        {
            Effargia = GameObject.FindGameObjectWithTag("Effargia");
            Effargia.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        tele = GameObject.Find("MuseumTele (2)");
        tele1 = GameObject.Find("MuseumTele (1)");
        tele2 = GameObject.Find("MuseumTele");
        if (movementEnabled)
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? baseSpeed * sprintMultiplier : baseSpeed;
            movementVector.x = Input.GetAxisRaw("Horizontal");
            movementVector.y = Input.GetAxisRaw("Vertical");
            movementVector = movementVector.normalized;
            if (movementVector != Vector2.zero)
            {
                lastMoveDirection = movementVector;
            }
            rb.velocity = new Vector2(currentSpeed * movementVector.x, currentSpeed * movementVector.y);
        }

        if (animalpoints > 11)
        {
            Effargia.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("RhythmMiniGame");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CastLightning();
        }


    }
    void CastLightning()
    {
        Vector2 offset = lastMoveDirection.normalized * 3.2f;
        Vector3 spawnPosition = transform.position + new Vector3(offset.x, offset.y, 0);

        GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);

        if (lastMoveDirection.x > 0)
            lightning.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (lastMoveDirection.x < 0)
            lightning.transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (lastMoveDirection.y > 0)
            lightning.transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (lastMoveDirection.y < 0)
            lightning.transform.rotation = Quaternion.Euler(0, 0, -90);

        Destroy(lightning, 0.3f);
    }

    void OnMove(InputValue value)
    {
        movementVector = value.Get<Vector2>();
        animator.SetBool("Walk_Side", !Mathf.Approximately(movementVector.x, 0));
        animator.SetBool("Walk_Front", !Mathf.Approximately(movementVector.y, 0));
        animator.SetBool("Walk_Back", !Mathf.Approximately(movementVector.y, 0));

        if (!Mathf.Approximately(movementVector.x, 0))
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
    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D prb = projectile.GetComponent<Rigidbody2D>();
        prb.velocity = lastMoveDirection * projectileSpeed;

        if (lastMoveDirection.x > 0)
            projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (lastMoveDirection.x < 0)
            projectile.transform.rotation = Quaternion.Euler(0, 0, 180);
        else if (lastMoveDirection.y > 0)
            projectile.transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (lastMoveDirection.y < 0)
            projectile.transform.rotation = Quaternion.Euler(0, 0, -90);

        Destroy(projectile, 2f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orb"))
        {
            collision.gameObject.SetActive(false);
            orb++;
            SFXSource.PlayOneShot(orbCollected);
            Debug.Log("Orbs Obtained: " + orb);
        }



        if (collision.gameObject.CompareTag("Sarai"))
        {
            StartCoroutine(DeactivateWaterAfterDelay());
        }

        // Check if the object collided with has the tag "barrels"
        if (collision.gameObject.CompareTag("Barrels"))
        {
            GameObject[] objectsToDisappear = GameObject.FindGameObjectsWithTag("Disappear");
            // Loop through each object and set it active
            foreach (GameObject obj in objectsToDisappear)
            {
                obj.SetActive(false);
            }

        }

        if (collision.gameObject.CompareTag("Curator"))
        {

            // tele = GameObject.Find("MuseumTele (2)");
            tele.transform.position = new Vector2(18, 43);
            tele1.transform.position = new Vector2(0, 32);
            tele2.transform.position = new Vector2(-14, 45);

            Debug.Log("CT status: " + curatetalk);
        }
        if (collision.gameObject.CompareTag("tellOFF"))
        {
            tele.transform.position = new Vector2(-200, -50);
            tele1.transform.position = new Vector2(-200, 0);
            tele2.transform.position = new Vector2(-200, 30);


            Debug.Log("CT status: " + curatetalk);
        }

        if (collision.gameObject.CompareTag("Suit_Animal"))
        {

            animalpoints++;
            Debug.Log("Animal points: " + animalpoints);
        }
        if (collision.gameObject.CompareTag("Bubbabob"))
        {
            bubbaPoints++;
            Debug.Log("Bubba points: " + bubbaPoints);
        }

        if (collision.gameObject.CompareTag("Block_Collider_1"))
        {
            if (animalpoints >= 7)
                collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Block_Collider_2") && bubbaPoints >= 2)
        {
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Block_Collider_3") && bubbaPoints >= 4)
        {
            collision.gameObject.SetActive(false);
        }


        if (!isTeleporting && collision.gameObject.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
            StartCoroutine(Teleport());
        }

    }

    private void OnTriggerExit2d(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Teleporter"))
        {
            if (collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
        }
    }

    private IEnumerator Teleport()
    {
        isTeleporting = true;

        if (currentTeleporter != null)
        {
            rb.transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
        }

        yield return new WaitForSeconds(teleportCooldown);
        isTeleporting = false;
    }

    private IEnumerator DeactivateWaterAfterDelay()
    {
        // Wait for 45 seconds
        yield return new WaitForSeconds(45f);

        // Find the GameObject tagged "Water"
        GameObject waterTilemap = GameObject.FindWithTag("Water");

        // If the GameObject exists, deactivate it
        if (waterTilemap != null)
        {
            waterTilemap.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No GameObject with the tag 'Water' found.");
        }
    }

}