using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for moving the ingame camera to the player.
/// </summary>
public class CameraController : MonoBehaviour
{
    public Transform m_lookAt; //the player object
    public float m_smoothTime = 0.1f; 
    public Vector3 m_cameraPosition;

    private Vector3 p_velocity;

    // Start is called before the first frame update
    private void Start()
    {
        p_velocity = Vector3.zero;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //follow the player with a slight delay
        Vector3 targetPosition = m_cameraPosition + m_lookAt.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref p_velocity, m_smoothTime);
    }
}
