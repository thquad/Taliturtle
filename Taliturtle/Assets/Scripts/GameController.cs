using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject m_player;
    public GameObject m_camera;

    private GameObject p_lookAtPosition;

    public void Start()
    {
        p_lookAtPosition = new GameObject();
        p_lookAtPosition.transform.Translate(new Vector3(0, 1, 0));
        ResetLevel();
    }

    public void ResetLevel()
    {

        //initialize everything to their startpositions
        m_camera.GetComponent<CameraController>().m_lookAt = p_lookAtPosition.transform;
        m_player.GetComponent<PlayerController>().RespawnPlayer();

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
    }

    private void Update()
    {
        //change stuff later to an observer pattern
        PlayerController playerController = m_player.GetComponent<PlayerController>();
        CameraController cameraController = m_camera.GetComponent<CameraController>();

        if (playerController.isFinished())
        {
            if (playerController.isOutOfBounds())
            {
                Vector3 pos = cameraController.m_lookAt.position;
                p_lookAtPosition.transform.position = Vector3.zero;
                p_lookAtPosition.transform.rotation = Quaternion.identity;
                p_lookAtPosition.transform.Translate(pos);

                cameraController.m_lookAt = p_lookAtPosition.transform;

                if (m_player.transform.position.y < -10)
                {
                    if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    else
                        SceneManager.LoadScene(0);
                }
            }

        }else if (playerController.isOutOfBounds())
        {
            ResetLevel();
            playerController.setOutOfBounds(false);
        }
    }
}
