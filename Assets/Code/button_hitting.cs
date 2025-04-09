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
            return; // Don’t spawn more than max allowed

        GameObject balloon = Instantiate(balloonPrefab, canvasRectTransform);
        activeBalloons++;

        float randomX = Random.Range(xRange.x, xRange.y);
        float randomY = Random.Range(yRange.x, yRange.y);
        balloon.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, randomY);

        balloon.GetComponent<Button>().onClick.AddListener(() =>
        {
            score++;
            UpdateUI();
            activeBalloons--; // decrease the count when popped
            Destroy(balloon);
        });
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
