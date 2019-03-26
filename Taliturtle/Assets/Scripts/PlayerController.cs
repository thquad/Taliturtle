using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The PlayerController class.
/// Pushes the collisionball, checks for triggercollisions and resets Transform.
/// </summary>
public class PlayerController : MonoBehaviour
{

    private Rigidbody p_rigidbody;
    public float m_speed;
    public Joystick m_joystick;

    private bool p_finished = false;
    private bool p_outOfBounds = false;
    private Vector3 p_spawnPosition;
    private float p_velocityY;

    // Awake is called before anything else
    void Awake()
    {
        p_rigidbody = GetComponent<Rigidbody>();
        p_rigidbody.maxAngularVelocity = float.MaxValue;
        p_velocityY = 0;

        p_spawnPosition = new Vector3(0, 10, 0);
    }

    // Update 
    private void FixedUpdate()
    {
        //get input
        float moveHorizontal = m_joystick.Horizontal;
        float moveVertical = m_joystick.Vertical;
        moveHorizontal *= m_speed;
        moveVertical *= m_speed;

        //push ball according to input
        p_rigidbody.AddForce(new Vector3(moveHorizontal,0,moveVertical));

        //if downward velocity changes to upward velocity, play hit sound
        if (p_velocityY < -1f && p_rigidbody.velocity.y > 1f)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }

        //save the last recorded velocity for the if statement directly above
        p_velocityY = p_rigidbody.velocity.y;
    }

    /// <summary>
    /// Collision event when player hits a trigger.
    /// </summary>
    /// <param name="other">The other object.</param>
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

    /// <summary>
    /// Respawns the player to startposition.
    /// </summary>
    public void RespawnPlayer()
    {
        transform.position = new Vector3(p_spawnPosition.x, p_spawnPosition.y, p_spawnPosition.z);
        p_rigidbody.velocity = Vector3.zero;
        p_rigidbody.angularVelocity = Vector3.zero;
    }

    //------------------------------------------------------------------- Getter Setter

    /// <summary>
    /// Setter p_finished.
    /// </summary>
    /// <param name="value">Value to set.</param>
    public void setFinished(bool value)
    {
        p_finished = value;
    }

    /// <summary>
    /// Getter p_finished.
    /// </summary>
    /// <returns>p_finished</returns>
    public bool isFinished()
    {
        return p_finished;
    }

    /// <summary>
    /// Setter p_outOfBounds.
    /// </summary>
    /// <param name="value">Value to set.</param>
    public void setOutOfBounds(bool value)
    {
        p_outOfBounds = value;
    }

    /// <summary>
    /// Getter p_outOfBounds.
    /// </summary>
    /// <returns>p_outOfBounds</returns>
    public bool isOutOfBounds()
    {
        return p_outOfBounds;
    }

}
