using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// The LoadingScreen class.
/// Script used by the LoadingScreen / MenuScreen.
/// Animates falling turtle and screen transitions.
/// </summary>
public class LoadingScreen : MonoBehaviour
{

    public GameObject m_turtle;
    public GameObject m_camera;
    public GameObject m_lvlselectButtonImage;
    public GameObject m_contButtonImage;
    public Button m_continueButton;
    public Button m_lvlselectButton;
    public float m_rotationSpeed;
    public float m_turtleSmoothTime;
    public float m_buttonSmoothTime;

    private Vector3 p_turtleOriginalPosition;

    //camera values for transition animation between scenes
    private Vector3 p_cameraVelocity;
    private Vector3 p_cameraStart;
    private Vector3 p_cameraEnd;

    //bools for screen change
    private bool p_nextScene;
    private bool p_selectLevel;
    private bool p_returnScreen;

    //Continue button animation values
    private Vector3 p_contButtonOutside;
    private Vector3 p_contButtonInside;
    private Vector3 p_contButtonVelocity;

    //level select button animation values
    private Vector3 p_lvlselectButtonOutside;
    private Vector3 p_lvlselectButtonInside;
    private Vector3 p_lvlselectButtonVelocity;

    // Start is called before the first frame update
    private void Start()
    {
        p_turtleOriginalPosition = m_turtle.transform.position;

        p_cameraVelocity = new Vector3();
        p_cameraStart = new Vector3(0, 15, -10);
        m_camera.transform.position = p_cameraStart;
        p_cameraEnd = new Vector3(0, -50, -10);

        p_nextScene = false;
        p_selectLevel = false;
        p_returnScreen = false;

        m_continueButton.onClick.AddListener(OnClickContinue);
        m_lvlselectButton.onClick.AddListener(OnClickLvlselect);
        
        //initialise everything outside the screen
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
    private void Update()
    {
        //return key pressed?
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            p_nextScene = true;
            p_returnScreen = true;
        }

        //turtle animation
        float sinUpDown = Mathf.Sin(Time.time*3);
        float sinLeftRight = Mathf.Sin(Time.time * 1);
        m_turtle.transform.position = p_turtleOriginalPosition;
        m_turtle.transform.Translate(new Vector3(sinLeftRight, sinUpDown, 0));
        m_turtle.transform.Rotate(new Vector3(0,1,0), Time.deltaTime * m_rotationSpeed, Space.World);
        m_turtle.transform.Rotate(new Vector3(1, 0, 0), sinUpDown*0.5f, Space.World);

        //button animation
        float sinButtonScale = Mathf.Sin(Time.time * 3.1f);
        m_continueButton.transform.localScale = new Vector3(1, 1, 1) *(sinButtonScale * sinButtonScale * 0.05f + 0.4475f);

        //camera animation
        if (p_nextScene)
        {
            m_camera.transform.position = Vector3.SmoothDamp(m_camera.transform.position, p_cameraEnd, ref p_cameraVelocity, m_turtleSmoothTime);

            //animate buttons fly out
            m_contButtonImage.transform.localPosition = Vector3.SmoothDamp(m_contButtonImage.transform.localPosition, p_contButtonOutside, ref p_contButtonVelocity, m_buttonSmoothTime);
            m_lvlselectButtonImage.transform.localPosition = Vector3.SmoothDamp(m_lvlselectButtonImage.transform.localPosition, p_lvlselectButtonOutside, ref p_lvlselectButtonVelocity, m_buttonSmoothTime);

            if (m_camera.transform.position.y < -15)
            {
                //choose wich scene to load
                if (p_returnScreen)
                    MemoryCard.LoadSplash();
                else if (p_selectLevel)
                    MemoryCard.LoadLevelSelection();
                else
                    MemoryCard.LoadSelectedLevel();
            }
        }
        else
        {
            //animate camera
            m_camera.transform.position = Vector3.SmoothDamp(m_camera.transform.position, new Vector3(0,0,-10), ref p_cameraVelocity, m_turtleSmoothTime);

            //animate buttons to fly in
            m_contButtonImage.transform.localPosition = Vector3.SmoothDamp(m_contButtonImage.transform.localPosition, p_contButtonInside, ref p_contButtonVelocity, m_buttonSmoothTime);
            m_lvlselectButtonImage.transform.localPosition = Vector3.SmoothDamp(m_lvlselectButtonImage.transform.localPosition, p_lvlselectButtonInside, ref p_lvlselectButtonVelocity, m_buttonSmoothTime);

            //animate buttons to wobble
            ScaleAnimateObject(0.6f,0.6f,m_contButtonImage);
            ScaleAnimateObject(0.8f,0.8f,m_lvlselectButtonImage);
        }
    }

    /// <summary>
    /// Method for button wobble.
    /// </summary>
    /// <param name="scaleX">Original x scale of object.</param>
    /// <param name="scaleY">Original y scale of object.</param>
    /// <param name="gameObject">Gameobject to wobble around.</param>
    private void ScaleAnimateObject(float scaleX, float scaleY, GameObject gameObject)
    {
        float sinButtonScale = Mathf.Sin(Time.time * 3.1f);
        gameObject.transform.localScale = new Vector3(scaleX, scaleY, 1) * (sinButtonScale * sinButtonScale * 0.05f + 1f);
    }

    /// <summary>
    /// Continue button event method.
    /// </summary>
    private void OnClickContinue()
    {
        p_nextScene = true;
    }

    /// <summary>
    /// Levelselect button event method.
    /// </summary>
    private void OnClickLvlselect()
    {
        p_nextScene = true;
        p_selectLevel = true;
    }
}
