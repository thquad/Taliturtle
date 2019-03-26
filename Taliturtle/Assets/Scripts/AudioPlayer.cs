using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AudioPlayer class.
/// Plays music, wont be destroyed by scene transitions.
/// </summary>
public class AudioPlayer : MonoBehaviour
{

    private void Awake()
    {
        GameObject[] audioPlayers = GameObject.FindGameObjectsWithTag("Audio");

        //if another Audio Player, destroy this, else, never destroy this
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
