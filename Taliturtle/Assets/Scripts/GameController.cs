using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject m_player;
    public GameObject m_camera;
    public Text m_bestTime;
    public Text m_currentTime;

    public float m_startCamera = -20;
    public float m_endCamera = 40;

    private GameObject p_lookAtPosition;
    private float p_cameraSmoothTime;

    private GameObject p_levelControl;

    private float p_timeInSeconds;
    private float p_bestTimeInSeconds;

    public void Start()
    {
        p_lookAtPosition = new GameObject();
        p_lookAtPosition.transform.Translate(new Vector3(0, 1, 0));
        p_levelControl = GameObject.Find("LevelControl");
        p_levelControl.GetComponent<LevelTilt>().m_playerHasControl = false;
        p_timeInSeconds = 0;

        //prepare start animation
        m_camera.transform.Translate(new Vector3(m_startCamera, 0, 0));
        m_camera.GetComponent<CameraController>().m_lookAt = p_lookAtPosition.transform;
        p_cameraSmoothTime = m_camera.GetComponent<CameraController>().m_smoothTime;
        m_camera.GetComponent<CameraController>().m_smoothTime = p_cameraSmoothTime * 2;

        //load best time
        p_bestTimeInSeconds = MemoryCard.LoadHighScore().timeInSeconds;
        if (p_bestTimeInSeconds != 0)
            m_bestTime.text = p_bestTimeInSeconds.ToString("f2");
        m_currentTime.text = p_timeInSeconds.ToString("f2");

        StartCoroutine(WaitForASecond());

    }

    public void ResetLevel()
    {

        //initialize everything to their startpositions
        m_camera.GetComponent<CameraController>().m_lookAt = p_lookAtPosition.transform;
        m_player.GetComponent<PlayerController>().RespawnPlayer();
        p_levelControl.GetComponent<LevelTilt>().m_playerHasControl = false;
        p_timeInSeconds = 0;
        m_currentTime.text = p_timeInSeconds.ToString("f2");

        StartCoroutine(CheckPlayerStart());
    }

    IEnumerator CheckPlayerStart()
    {
        Rigidbody playerBody = m_player.GetComponent<Rigidbody>();
        while (playerBody.velocity.y < 0.01)
        {
            yield return null;
        }
        m_camera.GetComponent<CameraController>().m_lookAt = m_player.transform;
        m_camera.GetComponent<CameraController>().m_smoothTime = p_cameraSmoothTime;
        p_levelControl.GetComponent<LevelTilt>().m_playerHasControl = true;
    }

    IEnumerator WaitForASecond()
    {
        yield return new WaitForSeconds(0.5f);
        ResetLevel();
    }

    private void Update()
    {
        //change stuff later to an observer pattern
        PlayerController playerController = m_player.GetComponent<PlayerController>();
        CameraController cameraController = m_camera.GetComponent<CameraController>();

        if (Input.GetKeyDown(KeyCode.Escape))
            MemoryCard.LoadMenu();

        if (playerController.isFinished())
        {
            p_levelControl.GetComponent<LevelTilt>().m_playerHasControl = false;

            if (p_timeInSeconds < p_bestTimeInSeconds || p_bestTimeInSeconds == 0)
            {
                MemoryCard.SaveHighscore(new Highscore(MemoryCard.GetScene(),p_timeInSeconds)); //TODO doesnt work
                m_bestTime.text = p_timeInSeconds.ToString("f2"); 
            }

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
            if (p_levelControl.GetComponent<LevelTilt>().m_playerHasControl)
            {
                p_timeInSeconds += Time.deltaTime;
                m_currentTime.text = p_timeInSeconds.ToString("f2");
                //m_bestTime.text = ""+1 / Time.deltaTime;
            }
        }
    }
}
