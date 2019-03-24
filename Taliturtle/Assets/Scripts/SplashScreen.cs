using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public GameObject m_turtle;
    public GameObject m_title;
    public GameObject m_camera;
    public GameObject m_buttonText;
    public Button m_button;
    public float m_smoothTime;

    private Vector3 p_turtleOriginalPosition;
    private Quaternion p_turtleOriginalRotation;

    private Vector3 p_titleOriginalPosition;
    private Quaternion p_titleOriginalRotation;

    private bool gameStart;
    private float p_velocityTurtle;
    private Vector3 p_velocityCamera;

    // Start is called before the first frame update
    void Start()
    {
        p_turtleOriginalPosition = m_turtle.transform.position;
        p_turtleOriginalRotation = m_turtle.transform.rotation;

        p_titleOriginalPosition = m_title.transform.position;
        p_titleOriginalRotation = m_title.transform.rotation;

        m_button.onClick.AddListener(OnClickDropDown);

        gameStart = false;
        p_velocityTurtle = 20;
        p_velocityCamera = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            //animation for letting the turtle fall down
            m_turtle.transform.Translate(0, p_velocityTurtle*Time.deltaTime, 0, Space.World);
            m_turtle.transform.Rotate(70 * Time.deltaTime, -170*Time.deltaTime, 0, Space.World);
            p_velocityTurtle -= Time.deltaTime*80;

            //follow the turtle with the camera after a certain time
            if(p_velocityTurtle<-20)
                m_camera.transform.position = Vector3.SmoothDamp(m_camera.transform.position, new Vector3(0,m_turtle.transform.position.y,-10), ref p_velocityCamera, m_smoothTime);

            //if camera doesnt see gui elements anymore, change scene
            if (m_camera.transform.position.y < -15)
            {
                SceneManager.LoadScene("menu_loadingscreen");
            }
            

        }
        else
        {
            //Animation to wobble the title and turtle up and down
            float sinUpDown = Mathf.Sin(Time.time * 3);
            float sinTitleUpDown = Mathf.Sin((Time.time + 1.8f) * 3);
            TranslateGameObjects(m_turtle, p_turtleOriginalPosition, sinUpDown);
            TranslateGameObjects(m_title, p_titleOriginalPosition, sinTitleUpDown);

            //animation to wobble the title and turtle left to right
            float sinLeftRight = Mathf.Sin(Time.time * 0.8f);
            RotateGameObjects(m_turtle, p_turtleOriginalRotation, sinLeftRight);
            RotateGameObjects(m_title, p_titleOriginalRotation, sinLeftRight);

            //animation to scale the "drop down" button up and down
            float sinButtonScale = Mathf.Sin(Time.time * 3.1f);
            m_buttonText.transform.localScale = new Vector3(0.7f, 0.8f, 1) * (sinButtonScale * sinButtonScale * 0.05f + 1f);

        }
    }

    public void TranslateGameObjects(GameObject gameObject, Vector3 originalPosition, float value)
    {
        gameObject.transform.position = originalPosition;
        gameObject.transform.Translate(0, value * 0.1f, 0, Space.World);
    }

    public void RotateGameObjects(GameObject gameObject, Quaternion originalRotation, float value)
    {
        gameObject.transform.rotation = originalRotation;
        gameObject.transform.Rotate(0, value * 10, 0, Space.World);
    }

    public void OnClickDropDown()
    {

        if (!gameStart)
        {
            m_turtle.transform.Rotate(0, 0, -60);
            gameObject.GetComponent<AudioSource>().Play();
        }
        gameStart = true;
        
    }
}
