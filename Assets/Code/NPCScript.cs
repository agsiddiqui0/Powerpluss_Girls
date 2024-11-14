using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class NPCScript : MonoBehaviour

{
    [SerializeField] DialogueRunner runner;
    [SerializeField] string dialogueName;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider) {
        runner.StartDialogue(dialogueName);
    }

    void OnTriggerExit2D(Collider2D collider)
    {

    }
}
