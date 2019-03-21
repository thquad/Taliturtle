using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleModel : MonoBehaviour
{

    public float m_rotationSpeed;

    private Quaternion p_rotation;
    

    // Start is called before the first frame update
    void Awake()
    {
        p_rotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        Vector3 angle = p_rotation.eulerAngles;
        angle.y = angle.y + 0.5f;
        p_rotation.eulerAngles = angle;
        transform.rotation = Quaternion.Slerp(transform.rotation, p_rotation, m_rotationSpeed);
    }
}
