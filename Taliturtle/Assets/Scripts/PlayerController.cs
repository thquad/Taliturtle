using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody p_rigidbody;
    public float m_speed;
    public Joystick m_joystick;

    // Start is called before the first frame update
    void Start()
    {
        p_rigidbody = GetComponent<Rigidbody>();
        p_rigidbody.maxAngularVelocity = float.MaxValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update 
    private void FixedUpdate()
    {
        if (transform.position.y < -5)
        {
            transform.position = new Vector3(0, 1f, 0);
            p_rigidbody.velocity = Vector3.zero;
            p_rigidbody.angularVelocity = Vector3.zero;

        }

        float moveHorizontal = m_joystick.Horizontal;
        float moveVertical = m_joystick.Vertical;

        moveHorizontal *= m_speed;
        moveVertical *= m_speed;

        p_rigidbody.AddForce(new Vector3(moveHorizontal,0,moveVertical));
        
    }
}
