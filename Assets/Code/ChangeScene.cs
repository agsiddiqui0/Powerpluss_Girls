using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ChangeSceneTrigger : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void OnTriggerEnter2D(Collider2D collision) {
        SceneManager.LoadScene(sceneName);
        FindObjectOfType<ScreenChanger>().FadeToScene(sceneName);
    }

}
