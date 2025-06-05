using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAutoDestroy : MonoBehaviour
{
    public float duration = 0.5f; // adjust as needed

    void Start()
    {
        Destroy(gameObject, duration);
    }
}
