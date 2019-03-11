using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTilt : MonoBehaviour
{

    public Joystick m_joystick;
    public float m_maxRotation = 20;
    public float m_smoothTime = 0.05f;

    private float p_horizontal;
    private float p_vertical;
    private Vector2 p_velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        float inputX = m_joystick.Horizontal * m_maxRotation;
        float inputY = m_joystick.Vertical * m_maxRotation;

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
