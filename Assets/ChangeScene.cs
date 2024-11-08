using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTrigger : MonoBehaviour
{

    public void GoToMiddle()
    {
        SceneManager.LoadScene("Middle");
    }
}
