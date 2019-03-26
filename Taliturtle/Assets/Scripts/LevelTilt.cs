using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The LevelTilt class.
/// Tilts the level around the player.
/// </summary>
public class LevelTilt : MonoBehaviour
{
    
    public float m_smoothTime = 0.05f;
    public bool m_playerHasControl = true;

    //player and controls
    private GameObject m_player;
    private Joystick m_joystick;

    //max angle 
    private float m_maxRotation = 30;

    // Start is called before the first frame update
    private void Start()
    {
        m_player = GameObject.Find("Player");
        m_joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
    }

    // FixedUpdate is called frame-rate independent in a timestep
    private void FixedUpdate()
    {
        float inputH = m_joystick.Horizontal * m_maxRotation;
        float inputV = m_joystick.Vertical * m_maxRotation;

        if (m_playerHasControl)
            RotateAroundPlayer(inputH, inputV);
        else
            RotateAroundPlayer(0, 0);
    }

    /// <summary>
    /// Rotates the level around the player.
    /// </summary>
    /// <param name="inputH">Horizontal input.</param>
    /// <param name="inputV">Vertical input.</param>
    private void RotateAroundPlayer(float inputH, float inputV)
    {

        //reset everything at first
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        //translate to playerposition so we can rotate the level around the player
        transform.Translate(m_player.transform.position); 

        //rotate
        transform.Rotate(new Vector3(0, 0, -1), inputH);
        transform.Rotate(new Vector3(1, 0, 0), inputV);

        //translate back
        transform.Translate(-m_player.transform.position);
        
    }

}
