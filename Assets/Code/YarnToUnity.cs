using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnToUnity : MonoBehaviour
{
 public DialogueRunner dialogueRunner;
    
    private InMemoryVariableStorage variableStorage;
    bool talkwfrog;

    public void Start()
    {
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();
    }

    public void FrogYapping(bool talkwfrog)
    {
        variableStorage.TryGetValue("$talked", out talkwfrog);
        //we can edit the value however we want here
        variableStorage.SetValue("$talk", talkwfrog);
    }


}
