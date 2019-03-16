using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    public bool triggered;

    public void Start()
    {
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        triggered = true;
    }
}
