using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class CodePadManager : MonoBehaviour
{

    public GameObject unlockableObject;
    public GameObject codepadPanel;
    public TMP_Text inputDisplay;
    private string playerInput = "";
    private string correctCode = "2904";

    public void OpenCodepad()
    {
        codepadPanel.SetActive(true);
        ResetInput();
        Time.timeScale = 0f; // Optional: pause the game while codepad is open
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenCodepad();
        }

        if (Input.GetKeyDown(KeyCode.L) && codepadPanel.activeSelf)
        {
            CloseCodepad();
        }
    }

    IEnumerator DelayAndCheckCode()
    {
        yield return new WaitForSecondsRealtime(1f); // Wait 1 second in real time
        CheckCode();
    }


    public void CloseCodepad()
    {
        codepadPanel.SetActive(false);
        Time.timeScale = 1f; // resume game
    }

    public void OnNumberButtonClicked(string number)
    {
        if (playerInput.Length >= 4) return;

        playerInput += number;
        inputDisplay.text = playerInput;

        if (playerInput.Length == 4)
        {
            StartCoroutine(DelayAndCheckCode());
        }
    }


    public int maxAttempts = 3;
    private int currentAttempts = 0;

    void CheckCode()
    {
        if (playerInput == correctCode)
        {
            inputDisplay.text = "UNLOCKED";
            if (unlockableObject != null)
            {
                unlockableObject.SetActive(false); // disable the object
            }

            Invoke("CloseCodepad", 1.2f);
        }
        else
        {
            currentAttempts++;
            if (currentAttempts >= maxAttempts)
            {
                inputDisplay.text = "LOCKED";
                Invoke("CloseCodepad", 2f);
            }
            else
            {
                StartCoroutine(ShowWrongCodeAndReset());
            }
        }
    }


    void ResetInput()
    {
        playerInput = "";
        inputDisplay.text = "";
    }

    IEnumerator ShowWrongCodeAndReset()
    {
        inputDisplay.text = "XXXX";
        yield return new WaitForSecondsRealtime(1f);
        ResetInput();
    }

}

