using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider collider)
    {
        if (collider.tag == "Player")
        {
            SceneManager.LoadScene(LoadSceneMode.Single);
        }
    }
}
