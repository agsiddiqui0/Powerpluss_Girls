using UnityEngine;

public class TargetExplosion : MonoBehaviour
{
    public GameObject explosionPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lightning"))
        {
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            Destroy(other.gameObject); // Destroy the lightning
            Destroy(gameObject);       // Destroy the target
        }
    }
}
