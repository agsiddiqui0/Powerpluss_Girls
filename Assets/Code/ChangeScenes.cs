using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public PlayerController playerController;

public class ChangeScenes : MonoBehaviour
{
  public void GoToSceneTwo()
  {
    SceneManager.LoadScene("Underground");
    playerController.AddListener(UnfreezeMovement);

  }
}
