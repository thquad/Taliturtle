using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public GameObject m_turtle;
    public GameObject m_title;
    public Button m_button;

    private Vector3 p_turtleOriginalPosition;
    private Quaternion p_turtleOriginalRotation;

    private Vector3 p_titleOriginalPosition;
    private Quaternion p_titleOriginalRotation;

    private bool gameStart;
    private float velocity;

    // Start is called before the first frame update
    void Start()
    {
        p_turtleOriginalPosition = m_turtle.transform.position;
        p_turtleOriginalRotation = m_turtle.transform.rotation;

        p_titleOriginalPosition = m_title.transform.position;
        p_titleOriginalRotation = m_title.transform.rotation;

        m_button.onClick.AddListener(OnClickDropDown);

        gameStart = false;
        velocity = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            m_turtle.transform.Translate(0, velocity*Time.deltaTime, 0, Space.World);
            m_turtle.transform.Rotate(70 * Time.deltaTime, -170*Time.deltaTime, 0, Space.World);

            velocity -= Time.deltaTime*80;

            if(m_turtle.transform.position.y < -12)
            {
                if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                else
                    SceneManager.LoadScene(0);
            }
        }
        else
        {
            float sinUpDown = Mathf.Sin(Time.time * 3);
            float sinTitleUpDown = Mathf.Sin((Time.time + 1.8f) * 3);
            TranslateGameObjects(m_turtle, p_turtleOriginalPosition, sinUpDown);
            TranslateGameObjects(m_title, p_titleOriginalPosition, sinTitleUpDown);

            float sinLeftRight = Mathf.Sin(Time.time * 0.8f);
            RotateGameObjects(m_turtle, p_turtleOriginalRotation, sinLeftRight);
            RotateGameObjects(m_title, p_titleOriginalRotation, sinLeftRight);

            float sinButtonScale = Mathf.Sin(Time.time * 3.1f);
            m_button.transform.localScale = new Vector3(1, 1, 1) * (sinButtonScale * sinButtonScale * 0.05f + 1f);
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
        }
        gameStart = true;
        
    }
}
