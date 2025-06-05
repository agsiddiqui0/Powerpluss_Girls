using UnityEngine;

public class LightningBehavior : MonoBehaviour
{
    public GameObject explosionPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target"))
        {
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, other.transform.position, Quaternion.identity);

                // Explicitly play the animation if it's not auto-playing
                Animator animator = explosion.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play("Explosion"); // use exact animation state name
                }
            }

            Destroy(other.gameObject); // remove target
            Destroy(gameObject);       // remove lightning
        }
    }
}