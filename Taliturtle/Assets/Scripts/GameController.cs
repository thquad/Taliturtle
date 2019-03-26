using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The GameController class.
/// Manages ingame events and gamelogic.
/// </summary>
public class GameController : MonoBehaviour
{
    public GameObject m_player;
    public GameObject m_camera;
    public Text m_bestTime;
    public Text m_currentTime;

    public float m_startCamera = -20;
    public float m_endCamera = 40;

    //cmaera
    private GameObject p_lookAtPosition;
    private float p_cameraSmoothTime;

    //level
    private GameObject p_levelControl;

    //gui
    private float p_timeInSeconds;
    private float p_bestTimeInSeconds;

    // Start is called before the first frame update
    private void Start()
    {
        p_lookAtPosition = new GameObject();
        p_lookAtPosition.transform.Translate(new Vector3(0, 1, 0));
        p_levelControl = GameObject.FindWithTag("LevelControl");
        p_levelControl.GetComponent<LevelTilt>().m_playerHasControl = false;
        p_timeInSeconds = 0;

        //prepare camera start animation for loading into the scene
        m_camera.transform.Translate(new Vector3(m_startCamera, 0, 0));
        m_camera.GetComponent<CameraController>().m_lookAt = p_lookAtPosition.transform;
        p_cameraSmoothTime = m_camera.GetComponent<CameraController>().m_smoothTime;
        m_camera.GetComponent<CameraController>().m_smoothTime = p_cameraSmoothTime * 2;

        //load highscore and show it
        p_bestTimeInSeconds = MemoryCard.LoadHighScore().timeInSeconds;
        if (p_bestTimeInSeconds != 0)
            m_bestTime.text = p_bestTimeInSeconds.ToString("f2");
        m_currentTime.text = p_timeInSeconds.ToString("f2");

        StartCoroutine(WaitForASecond());

    }

    /// <summary>
    /// Resets the state of the level.
    /// </summary>
    public void ResetLevel()
    {
        //initialize everything to their startpositions
        m_camera.GetComponent<CameraController>().m_lookAt = p_lookAtPosition.transform;
        m_player.GetComponent<PlayerController>().RespawnPlayer();
        p_levelControl.GetComponent<LevelTilt>().m_playerHasControl = false;
        p_timeInSeconds = 0;
        m_currentTime.text = p_timeInSeconds.ToString("f2");

        //start a thread to check when player hits the ground
        StartCoroutine(CheckPlayerStart());
    }

    /// <summary>
    /// Checks if the player has touched ground and then starts the game.
    /// </summary>
    /// <returns>IEnumerator, which is needed for Courotines.</returns>
    IEnumerator CheckPlayerStart()
    {
        //<0.01f means that the player is still falling
        Rigidbody playerBody = m_player.GetComponent<Rigidbody>();
        while (playerBody.velocity.y < 0.01f)
        {
            yield return null;
        }

        //make the camera follow the player and give control
        m_camera.GetComponent<CameraController>().m_lookAt = m_player.transform;
        m_camera.GetComponent<CameraController>().m_smoothTime = p_cameraSmoothTime;
        p_levelControl.GetComponent<LevelTilt>().m_playerHasControl = true;
    }

    /// <summary>
    /// Waits for some time.
    /// </summary>
    /// <returns>IEnumerator, which is needed for Courotines.</returns>
    IEnumerator WaitForASecond()
    {
        yield return new WaitForSeconds(0.5f);
        ResetLevel();
    }

    // Update is called once per frame
    private void Update()
    {
        //todo change stuff later to an observer pattern
        PlayerController playerController = m_player.GetComponent<PlayerController>();
        CameraController cameraController = m_camera.GetComponent<CameraController>();

        //check if the back button has been pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            MemoryCard.LoadMenu();

        //has the player beaten the level / fallen outside the level / still playing
        if (playerController.isFinished())
        {
            //remove player control because he has beaten the level
            p_levelControl.GetComponent<LevelTilt>().m_playerHasControl = false;

            //update the highscore if the player was faster
            if (p_timeInSeconds < p_bestTimeInSeconds || p_bestTimeInSeconds == 0)
            {
                MemoryCard.SaveHighscore(new Highscore(MemoryCard.GetScene(),p_timeInSeconds)); //TODO doesnt work
                m_bestTime.text = p_timeInSeconds.ToString("f2"); 
            }

            //start cameranimation once player is out of bounds
            if (playerController.isOutOfBounds())
            {
                p_lookAtPosition.transform.position = new Vector3(m_endCamera, 0, 0);
                m_camera.GetComponent<CameraController>().m_lookAt = p_lookAtPosition.transform;
                m_camera.GetComponent<CameraController>().m_smoothTime = p_cameraSmoothTime * 4;

                if (m_camera.transform.position.x > m_endCamera/2)
                {
                    MemoryCard.LoadNextLevel();
                }
            }

        }else if (playerController.isOutOfBounds())
        {
            ResetLevel();
            playerController.setOutOfBounds(false);
        }
        else
        {
            //start the timer once the player has control
            if (p_levelControl.GetComponent<LevelTilt>().m_playerHasControl)
            {
                p_timeInSeconds += Time.deltaTime;
                m_currentTime.text = p_timeInSeconds.ToString("f2");
            }
        }
    }
}
