using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    public GameObject m_turtle;
    public GameObject m_camera;
    public GameObject m_lvlselectButtonImage;
    public GameObject m_contButtonImage;
    public Button m_continueButton;
    public float m_rotationSpeed;
    public float m_turtleSmoothTime;
    public float m_buttonSmoothTime;

    private Vector3 p_turtleOriginalPosition;

    private Vector3 p_cameraVelocity;
    private Vector3 p_cameraStart;
    private Vector3 p_cameraEnd;

    private bool p_startLevel;

    private Vector3 p_contButtonOutside;
    private Vector3 p_contButtonInside;
    private Vector3 p_contButtonVelocity;

    private Vector3 p_lvlselectButtonOutside;
    private Vector3 p_lvlselectButtonInside;
    private Vector3 p_lvlselectButtonVelocity;

    // Start is called before the first frame update
    void Start()
    {
        p_turtleOriginalPosition = m_turtle.transform.position;

        p_cameraVelocity = new Vector3();
        p_cameraStart = new Vector3(0, 15, -10);
        m_camera.transform.position = p_cameraStart;
        p_cameraEnd = new Vector3(0, -50, -10);

        p_startLevel = false;

        m_continueButton.onClick.AddListener(OnClickContinue);

        p_contButtonVelocity = new Vector3();
        p_contButtonInside = m_contButtonImage.transform.localPosition;
        p_lvlselectButtonVelocity = new Vector3();
        p_lvlselectButtonInside = m_lvlselectButtonImage.transform.localPosition;

        p_contButtonOutside = m_contButtonImage.transform.localPosition;
        p_contButtonOutside.x = p_contButtonOutside.x - 20;
        m_contButtonImage.transform.localPosition = p_contButtonOutside;

        p_lvlselectButtonOutside = m_lvlselectButtonImage.transform.localPosition;
        p_lvlselectButtonOutside.x = p_lvlselectButtonOutside.x + 20;
        m_lvlselectButtonImage.transform.localPosition = p_lvlselectButtonOutside;
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
        m_continueButton.transform.localScale = new Vector3(1, 1, 1) *(sinButtonScale * sinButtonScale * 0.05f + 0.4475f);

        //camera animation
        if (p_startLevel)
        {
            m_camera.transform.position = Vector3.SmoothDamp(m_camera.transform.position, p_cameraEnd, ref p_cameraVelocity, m_turtleSmoothTime);

            //animate buttons fly out
            m_contButtonImage.transform.localPosition = Vector3.SmoothDamp(m_contButtonImage.transform.localPosition, p_contButtonOutside, ref p_contButtonVelocity, m_buttonSmoothTime);
            m_lvlselectButtonImage.transform.localPosition = Vector3.SmoothDamp(m_lvlselectButtonImage.transform.localPosition, p_lvlselectButtonOutside, ref p_lvlselectButtonVelocity, m_buttonSmoothTime);

            if (m_camera.transform.position.y < -15)
            {
                StaticInformation.LoadCurrentProgress();
            }
        }
        else
        {
            m_camera.transform.position = Vector3.SmoothDamp(m_camera.transform.position, new Vector3(0,0,-10), ref p_cameraVelocity, m_turtleSmoothTime);

            //animate buttons to fly in
            m_contButtonImage.transform.localPosition = Vector3.SmoothDamp(m_contButtonImage.transform.localPosition, p_contButtonInside, ref p_contButtonVelocity, m_buttonSmoothTime);
            m_lvlselectButtonImage.transform.localPosition = Vector3.SmoothDamp(m_lvlselectButtonImage.transform.localPosition, p_lvlselectButtonInside, ref p_lvlselectButtonVelocity, m_buttonSmoothTime);
        }
    }

    private void OnClickContinue()
    {
        p_startLevel = true;
    }
}
