using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ChangeSceneTrigger : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void OnTriggerEnter2D(Collider2D collision) {
        SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
        if (sceneTransition != null)
        {
            sceneTransition.FadeOutAndLoadScene(sceneName);
        }
        FindObjectOfType<ScreenChanger>().FadeToScene(sceneName);
    }

}
