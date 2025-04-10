using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class button_hitting : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject balloonPrefab;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private int numberOfBalloons = 10;

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float gameDuration = 30f;

    [SerializeField] private GameObject hitBackground;
    [SerializeField] private GameObject missBackground;
    [SerializeField] private float bgFlashDuration = 0.2f;


    private Vector2 xRange;
    private Vector2 yRange;

    private int score = 0;
    private float timeLeft;
    private bool gameRunning = false;

    private int activeBalloons = 0;
    private int maxBalloons = 5;

    void Start()
    {
        // Calculate bounds from canvas size
        float halfWidth = canvasRectTransform.rect.width / 2f - 100;
        float halfHeight = canvasRectTransform.rect.height / 2f - 100;

        if (scoreText != null) scoreText.gameObject.SetActive(false);
        if (timerText != null) timerText.gameObject.SetActive(false);

        xRange = new Vector2(-halfWidth, halfWidth);
        yRange = new Vector2(-halfHeight, halfHeight);

        StartCoroutine(StartMinigame());
    }

    void OnTriggerEnter(Collider other)
    {

    }

    IEnumerator StartMinigame()
    {
        gameRunning = true;
        score = 0;
        timeLeft = gameDuration;

        // Show UI again
        if (scoreText != null) scoreText.gameObject.SetActive(true);
        if (timerText != null) timerText.gameObject.SetActive(true);

        UpdateUI();
        StartCoroutine(CountdownTimer());

        while (gameRunning)
        {
            if (activeBalloons < maxBalloons)
            {
                SpawnBalloon();
            }

            float spawnDelay = Mathf.Lerp(1.0f, 0.2f, 1f - (timeLeft / gameDuration));
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator FlashBackground(GameObject bgObject)
    {
        if (bgObject == null) yield break;

        bgObject.SetActive(true);
        yield return new WaitForSeconds(bgFlashDuration);
        bgObject.SetActive(false);
    }



    IEnumerator BalloonLifetime(GameObject balloon, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        if (balloon != null) // balloon was not clicked
        {
            score = Mathf.Max(0, score - 1);
            UpdateUI();
            activeBalloons--;
            Destroy(balloon);

            StartCoroutine(FlashBackground(missBackground));
        }

    }

    IEnumerator AnimatePopIn(Transform obj, float duration)
    {
        float timer = 0f;
        Vector3 targetScale = Vector3.one;

        while (timer < duration)
        {
            obj.localScale = Vector3.Lerp(Vector3.zero, targetScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        obj.localScale = targetScale; // make sure it finishes exactly at 1
    }


    IEnumerator CountdownTimer()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            UpdateUI();
        }

        EndMinigame();
    }

    void SpawnBalloon()
    {
        if (activeBalloons >= maxBalloons)
            return;

        GameObject balloon = Instantiate(balloonPrefab, canvasRectTransform);
        activeBalloons++;
        balloon.transform.localScale = Vector3.zero;
        StartCoroutine(AnimatePopIn(balloon.transform, 0.3f));


        float randomX = Random.Range(xRange.x, xRange.y);
        float randomY = Random.Range(yRange.x, yRange.y);
        balloon.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, randomY);

        balloon.GetComponent<Button>().onClick.AddListener(() =>
        {
            score++;
            UpdateUI();
            activeBalloons--;
            Destroy(balloon);

            StartCoroutine(FlashBackground(hitBackground));
        });


        // Start timeout countdown
        StartCoroutine(BalloonLifetime(balloon, 2f)); // 3 seconds timeout
    }


    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
    }

    void EndMinigame()
    {
        gameRunning = false;

        foreach (Transform child in canvasRectTransform)
        {
            if (child.CompareTag("Balloon"))
                Destroy(child.gameObject);
        }

        // Hide UI
        if (scoreText != null) scoreText.gameObject.SetActive(false);
        if (timerText != null) timerText.gameObject.SetActive(false);

        Debug.Log("Game Over! Final Score: " + score);
    }

}
