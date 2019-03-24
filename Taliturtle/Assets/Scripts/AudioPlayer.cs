using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    private void Awake()
    {
        GameObject[] audioPlayers = GameObject.FindGameObjectsWithTag("Audio");
        if (audioPlayers.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
