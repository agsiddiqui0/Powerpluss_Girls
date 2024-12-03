/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterYSBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using Yarn.Unity;
using Yarn;

Yarn.Unity.InMemoryVariableStorage variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
float testVariable;
variableStorage.TryGetValue("$capture", out testVariable);

variableStorage.SetValue("$capture", testVariable + 1);
public class TeleporterYSBase : MonoBehaviour
{
    [SerializeField] private Transform destination;
    public Transform GetDestination()
    {
        return destination;
    }

}
*/