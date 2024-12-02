using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class REQ_GoToMM : MonoBehaviour
{
    public void GoToMM() {
        SceneManager.LoadScene("Main Menu");
    }
}
