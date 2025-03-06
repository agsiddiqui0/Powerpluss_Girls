using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class animal_yarn : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    private InMemoryVariableStorage variableStorage;

    public void CheckVariableAfterDialogue()
    {
        if (variableStorage.TryGetValue("$kang", out object value))
        {
            Debug.Log($"After dialogue, $kang is: {value}");
        }
    }


    public void Start()
    {
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();

        dialogueRunner.onDialogueComplete.AddListener(CheckVariableAfterDialogue);

        if (variableStorage.TryGetValue("$kang", out object value))
        {
            Debug.Log($"Variable found at Start: {value}");
        }
        else
        {
            Debug.Log("Variable $kang not found in storage!");
        }

    }

    //public bool KangarooAttack(bool bo) {   //variableStorage.SetValue("$kang", kangr); }


    public void Update()
    {

    }



}


