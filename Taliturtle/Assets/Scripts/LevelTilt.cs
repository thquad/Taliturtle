using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTilt : MonoBehaviour
{
    public GameObject m_player;
    public Joystick m_joystick;
    public float m_maxRotation = 20;
    public float m_smoothTime = 0.05f;

    private float p_horizontal;
    private float p_vertical;
    private Vector2 p_velocity = Vector2.zero;

    private Vector3 p_originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        p_originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        float inputX = m_joystick.Horizontal * m_maxRotation;
        float inputY = m_joystick.Vertical * m_maxRotation;

        RotateAroundPlayer(inputX, inputY);
        //FixedRotationPointInput(inputX, inputY);
    }

    void RotateAroundPlayer(float inputX, float inputY)
    {
        //reset everything at first
        transform.position = Vector3.zero;
        transform.rotation = new Quaternion();

        transform.Translate(m_player.transform.position); //translate to playerposition so we can rotate the level around the player

        transform.Rotate(new Vector3(0, 0, -1), inputX);
        transform.Rotate(new Vector3(1, 0, 0), inputY);

        /*
         * transform.position = Vector3.zero;
         * The line above wont work because of the rotation the level might be above the (0,0,0) vector. (Imagine the ball pushing the level over) 
         */
        transform.Translate(-m_player.transform.position);
        transform.Translate(p_originalPosition);

    }

    void FixedRotationPointInput(float inputX, float inputY)
    {
        Vector2 input = new Vector2(inputX, inputY);
        Vector2 lastInput = new Vector2(p_horizontal, p_vertical);
        Vector2 calculatedInput = Vector2.SmoothDamp(lastInput, input, ref p_velocity, m_smoothTime);

        p_horizontal = calculatedInput.x;
        p_vertical = calculatedInput.y;

        transform.rotation = new Quaternion();
        transform.Rotate(new Vector3(0, 0, -1), p_horizontal);
        transform.Rotate(new Vector3(1, 0, 0), p_vertical);
    }
}
