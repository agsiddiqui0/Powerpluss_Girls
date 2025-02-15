using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenChanger : MonoBehaviour
{
    public float fadeDuration = 1f;
    private Image fadeImage;

    void Awake()
    {
        fadeImage = GetComponent<Image>();
        StartCoroutine(FadeFromBlack());  // Fade in when scene starts
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));  // Call this to fade out and switch scenes
    }

    IEnumerator FadeFromBlack()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);  // Load the new scene after fade out
    }
}
