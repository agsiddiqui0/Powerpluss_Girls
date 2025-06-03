using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RhythmMiniGame : MonoBehaviour
{
    [SerializeField] private Button gameButton0;
    [SerializeField] private Button gameButton1;
    [SerializeField] private Button gameButton2;
    [SerializeField] private Button gameButton3;
    [SerializeField] private Button gameButton4;

    [SerializeField] private Button playerButton0;
    [SerializeField] private Button playerButton1;
    [SerializeField] private Button playerButton2;
    [SerializeField] private Button playerButton3;
    [SerializeField] private Button playerButton4;

    [SerializeField] private Sprite litSprite;
    [SerializeField] private Sprite unlitSprite;

    [SerializeField] private GameObject correctImage;
    [SerializeField] private GameObject wrongImage;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private float feedbackDisplayTime = 1.5f;

    private List<int> currentSequence = new List<int>();
    private int playerIndex = 0;
    private int round = 1;
    private bool inputEnabled = false;
    private bool gameStarted = false;


    void Start()
    {
        if (correctImage != null)
            correctImage.SetActive(false);
        if (wrongImage != null)
            wrongImage.SetActive(false);
        if (gameUI != null)
            gameUI.SetActive(true);

        InitButtons();
        //StartCoroutine(PlayRound());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !gameStarted)
        {
            gameStarted = true;
            StartCoroutine(PlayRound());
        }
    }


    void InitButtons()
    {
        playerButton0.onClick.AddListener(() => PlayerInput(0));
        playerButton1.onClick.AddListener(() => PlayerInput(1));
        playerButton2.onClick.AddListener(() => PlayerInput(2));
        playerButton3.onClick.AddListener(() => PlayerInput(3));
        playerButton4.onClick.AddListener(() => PlayerInput(4));
    }

    IEnumerator PlayRound()
    {
        inputEnabled = false;
        currentSequence.Clear();

        int sequenceLength = GetSequenceLengthForRound(round);

        for (int i = 0; i < sequenceLength; i++)
        {
            int randomIndex = Random.Range(0, 5);
            currentSequence.Add(randomIndex);
            yield return StartCoroutine(FlashButton(GetGameButton(randomIndex)));
            yield return new WaitForSeconds(0.2f);
        }

        playerIndex = 0;
        inputEnabled = true;
    }

    IEnumerator FlashButton(Button btn)
    {
        if (btn == null)
            yield break;

        btn.image.sprite = litSprite;
        yield return new WaitForSeconds(0.5f);
        btn.image.sprite = unlitSprite;
    }

    void PlayerInput(int index)
    {
        if (!inputEnabled) return;

        if (index == currentSequence[playerIndex])
        {
            playerIndex++;
            if (playerIndex >= currentSequence.Count)
            {
                inputEnabled = false;
                StartCoroutine(ShowCorrectAndContinue());
            }
        }
        else
        {
            inputEnabled = false;
            StartCoroutine(ShowWrongAndRetry());
        }
    }

    IEnumerator ShowCorrectAndContinue()
    {
        if (gameUI != null)
            gameUI.SetActive(false);
        if (correctImage != null)
            correctImage.SetActive(true);
        yield return new WaitForSeconds(feedbackDisplayTime);
        if (correctImage != null)
            correctImage.SetActive(false);
        if (gameUI != null)
            gameUI.SetActive(true);

        round++;

        if (round > 3)
        {
            SceneManager.LoadScene("Underground");
        }
        else
        {
            StartCoroutine(PlayRound());
        }
    }

    IEnumerator ShowWrongAndRetry()
    {
        if (gameUI != null)
            gameUI.SetActive(false);
        if (wrongImage != null)
            wrongImage.SetActive(true);
        yield return new WaitForSeconds(feedbackDisplayTime);
        if (wrongImage != null)
            wrongImage.SetActive(false);
        if (gameUI != null)
            gameUI.SetActive(true);

        StartCoroutine(PlayRound());
    }

    int GetSequenceLengthForRound(int r)
    {
        if (r == 1) return 3;
        if (r == 2) return 4;
        return 5;
    }

    Button GetGameButton(int index)
    {
        return index switch
        {
            0 => gameButton0,
            1 => gameButton1,
            2 => gameButton2,
            3 => gameButton3,
            4 => gameButton4,
            _ => null
        };
    }
}
