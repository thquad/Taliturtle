using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject m_player;
    public float m_smoothTime = 0.1f;

    private Vector3 p_velocity;

    // Start is called before the first frame update
    void Start()
    {
        p_velocity = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(0, 13, -9) + m_player.transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref p_velocity, m_smoothTime);

        //transform.position = m_player.transform.position + p_offset;
    }
}
