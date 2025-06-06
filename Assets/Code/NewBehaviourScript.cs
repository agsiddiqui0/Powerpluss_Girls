using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportTrigger : MonoBehaviour
{
    public GameObject Player;
    public Transform teleportTarget;
    public CanvasGroup blackPanel;
    public float fadeDuration = 0.5f;

    private bool canTrigger = false;
    private bool isFading = false;

    void OnTriggerEnter(Collider other)
    {
        if (canTrigger && other.gameObject == Player && !isFading)
        {
            StartCoroutine(HandleTeleport());
        }
    }

    public void EnableTriggerAfterDialogue()
    {
        canTrigger = true;
    }

    IEnumerator HandleTeleport()
    {
        isFading = true;

        // Freeze time
        Time.timeScale = 0f;

        // Fade in black panel
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            blackPanel.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        blackPanel.alpha = 1;

        // Move player
        Player.transform.position = teleportTarget.position;

        // Fade out player (optional)
        SpriteRenderer sr = Player.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            t = 0;
            Color original = sr.color;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                sr.color = new Color(original.r, original.g, original.b, Mathf.Lerp(1, 0, t / fadeDuration));
                yield return null;
            }
        }

        // Unfreeze time
        Time.timeScale = 1f;

        yield return new WaitForSeconds(0.5f);

        // Fade panel back to clear if needed
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            blackPanel.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        blackPanel.alpha = 0;
    }
}