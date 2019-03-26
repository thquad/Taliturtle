using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TurtleModel class.
/// Rotates the turtle so it slides instead of rolls.
/// </summary>
public class TurtleModel : MonoBehaviour
{

    public float m_rotationSpeed;

    private Quaternion p_rotation; //original rotation
    
    // Start is called before the first frame update
    private void Awake()
    {
        p_rotation = transform.rotation;
    }

    // FixedUpdate is called frame-rate independent in a timestep
    private void FixedUpdate()
    {
        //slowly rotates the turtle back to original rotation, so that the turtle doesnt "fall over"
        Vector3 angle = p_rotation.eulerAngles;
        angle.y = angle.y + 0.5f;
        p_rotation.eulerAngles = angle;
        transform.rotation = Quaternion.Slerp(transform.rotation, p_rotation, m_rotationSpeed);
    }
}
