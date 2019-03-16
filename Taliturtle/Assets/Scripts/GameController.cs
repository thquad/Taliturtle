using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Goal m_goal;
    public GameObject m_player;
    public GameObject m_camera;

    private GameObject p_lookAtPosition;

    public void Start()
    {
        p_lookAtPosition = new GameObject();
        p_lookAtPosition.transform.Translate(new Vector3(0, 1, 0));

        //start animation
        /*
        m_camera.transform.Translate(new Vector3(0, 20, 0));
        m_camera.GetComponent<CameraController>().m_smoothTime = 1;
        */
        m_camera.GetComponent<CameraController>().m_lookAt = p_lookAtPosition.transform;
        

        StartCoroutine(checkPlayerStart());
    }

    IEnumerator checkPlayerStart()
    {
        Rigidbody playerBody = m_player.GetComponent<Rigidbody>();
        while (playerBody.velocity.y < 0.01)
        {
            yield return null;
        }
        m_camera.GetComponent<CameraController>().m_lookAt = m_player.transform;
        m_camera.GetComponent<CameraController>().m_smoothTime = 0.2f;
    }
}
