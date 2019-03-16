﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private Rigidbody p_rigidbody;
    public float m_speed;
    public Joystick m_joystick;

    private bool p_finished = false;
    private bool p_outOfBounds = false;
    private Vector3 p_spawnPosition;

    // Start is called before the first frame update
    void Awake()
    {
        p_rigidbody = GetComponent<Rigidbody>();
        p_rigidbody.maxAngularVelocity = float.MaxValue;

        p_spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update 
    private void FixedUpdate()
    {

        float moveHorizontal = m_joystick.Horizontal;
        float moveVertical = m_joystick.Vertical;

        moveHorizontal *= m_speed;
        moveVertical *= m_speed;

        p_rigidbody.AddForce(new Vector3(moveHorizontal,0,moveVertical));
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            setFinished(true);
        }

        if (other.CompareTag("OutOfBounds"))
        {
            setOutOfBounds(true);
        }
    }

    public void RespawnPlayer()
    {
        transform.position = new Vector3(p_spawnPosition.x, p_spawnPosition.y, p_spawnPosition.z);
        p_rigidbody.velocity = Vector3.zero;
        p_rigidbody.angularVelocity = Vector3.zero;
    }

    //------------------------------------------------------------------- Getter Setter

    public void setFinished(bool value)
    {
        p_finished = value;
    }

    public bool isFinished()
    {
        return p_finished;
    }

    public void setOutOfBounds(bool value)
    {
        p_outOfBounds = value;
    }

    public bool isOutOfBounds()
    {
        return p_outOfBounds;
    }

}
