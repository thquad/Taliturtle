using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    public GameObject m_turtle;
    public GameObject m_camera;
    public Button m_buttonContinue;
    public float m_rotationSpeed;
    public float m_turtleSmoothTime;
    public float m_buttonSmoothTime;

    private Vector3 p_turtleOriginalPosition;

    private Vector3 p_cameraVelocity;
    private Vector3 p_cameraStart;
    private Vector3 p_cameraEnd;

    private bool p_startLevel;

    private Vector3 p_butContOutsidePos;
    private Vector3 p_butContOriginalPos;
    private Vector3 p_butContVelocity;

    // Start is called before the first frame update
    void Start()
    {
        p_turtleOriginalPosition = m_turtle.transform.position;

        p_cameraVelocity = new Vector3();
        p_cameraStart = new Vector3(0, 15, -10);
        m_camera.transform.position = p_cameraStart;
        p_cameraEnd = new Vector3(0, -50, -10);

        p_startLevel = false;

        m_buttonContinue.onClick.AddListener(OnClickContinue);
        p_butContVelocity = new Vector3();
        p_butContOriginalPos = m_buttonContinue.transform.position;

        p_butContOutsidePos = new Vector3(m_buttonContinue.transform.position.x, m_buttonContinue.transform.position.y, m_buttonContinue.transform.position.z);
        p_butContOutsidePos.x = p_butContOutsidePos.x - 500;
        m_buttonContinue.transform.position = p_butContOutsidePos;
    }

    // Update is called once per frame
    void Update()
    {
        //turtle animation
        float sinUpDown = Mathf.Sin(Time.time*3);
        float sinLeftRight = Mathf.Sin(Time.time * 1);
        m_turtle.transform.position = p_turtleOriginalPosition;
        m_turtle.transform.Translate(new Vector3(sinLeftRight, sinUpDown, 0));
        m_turtle.transform.Rotate(new Vector3(0,1,0), Time.deltaTime * m_rotationSpeed, Space.World);
        m_turtle.transform.Rotate(new Vector3(1, 0, 0), sinUpDown, Space.World);

        //button animation
        float sinButtonScale = Mathf.Sin(Time.time * 3.1f);
        m_buttonContinue.transform.localScale = new Vector3(1, 1, 1) *(sinButtonScale * sinButtonScale * 0.05f + 0.4475f);

        //camera animation
        if (p_startLevel)
        {
            m_camera.transform.position = Vector3.SmoothDamp(m_camera.transform.position, p_cameraEnd, ref p_cameraVelocity, m_turtleSmoothTime);
            m_buttonContinue.transform.position = Vector3.SmoothDamp(m_buttonContinue.transform.position, p_butContOutsidePos, ref p_butContVelocity, m_buttonSmoothTime);

            if (m_camera.transform.position.y < -15)
            {
                if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                else
                    SceneManager.LoadScene(0);
            }
        }
        else
        {
            m_camera.transform.position = Vector3.SmoothDamp(m_camera.transform.position, new Vector3(0,0,-10), ref p_cameraVelocity, m_turtleSmoothTime);
            m_buttonContinue.transform.position = Vector3.SmoothDamp(m_buttonContinue.transform.position, p_butContOriginalPos, ref p_butContVelocity, m_buttonSmoothTime);
        }
    }

    private void OnClickContinue()
    {
        p_startLevel = true;
    }
}
