using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button m_contButton;
    public GameObject m_camera;
    public GameObject m_continuePlane;
    public GameObject m_levelselectPlane;
    public Text m_text;

    private Vector3 p_continuePlaneScale;
    private Vector3 p_levelselectPlaneScale;

    private Vector2 startPos;
    private Vector2 direction;
    private float p_triggerDistance;

    private GameObject p_currentMapPreview;
    private float p_lastLevelChange;

    private Vector3 p_cameraVelocity;
    private Vector3 p_cameraInsidePosition;
    private Vector3 p_cameraOutsidePosition;
    private float p_cameraSmoothTime;

    private Vector3 p_mapMiddlePosition = new Vector3(0, 1, 5);
    private Vector3 p_mapLeftPosition = new Vector3(-20, 1, 5);
    private Vector3 p_mapRightPosition = new Vector3(20, 1, 5);
    private Vector3 p_mapGoToThisPosition = new Vector3(0, 1, 5);
    private float p_mapSmoothTime = 0.1f;
    private Vector3 p_mapVelocity = new Vector3();

    private bool p_nextScene = false;
    private bool p_returnScene = false;

    private string p_textString = "";

    private void Start()
    {
        p_continuePlaneScale = m_continuePlane.transform.localScale;
        p_levelselectPlaneScale = m_levelselectPlane.transform.localScale;

        m_contButton.onClick.AddListener(OnClickContinue);
        p_cameraInsidePosition = m_camera.transform.position;
        m_camera.transform.Translate(-20, 0, 0);
        p_cameraOutsidePosition = m_camera.transform.position;

        p_cameraSmoothTime = 0.3f;

        p_lastLevelChange = 1;
        p_triggerDistance = Screen.width*0.1f; //if 10% of the screen has been touchmoved
        LoadPreview(MemoryCard.GetSelectedLevelIndex());
    }

    // Update is called once per frame
    void Update()
    {
        //return key pressed?
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            p_nextScene = true;
            p_returnScene = true;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    direction = touch.position - startPos;
                    break;
                case TouchPhase.Ended:
                    direction.x = 0;
                    direction.y = 0;
                    break;
            }
        }

        if (Mathf.Abs(direction.x) > p_triggerDistance)
        {
            if (Time.time - p_lastLevelChange > 0.3f)
            {
                p_lastLevelChange = Time.time;

                if (direction.x < 0)
                {
                    MemoryCard.AddToSelectedLevel(+1);
                    p_mapGoToThisPosition = p_mapLeftPosition;
                }
                else
                {
                    MemoryCard.AddToSelectedLevel(-1);
                    p_mapGoToThisPosition = p_mapRightPosition;
                }
                    
            }
        }

        if(p_nextScene && m_camera.transform.position.x < -19)
        {
            if (p_returnScene)
                MemoryCard.LoadMenu();
            else
                MemoryCard.LoadSelectedLevel();
        }

        AnimateScreenTransitions();
        AnimateMapTransition();
        AnimateScreen();

        if(p_currentMapPreview.transform.position.x < -19 || p_currentMapPreview.transform.position.x > 19)
        {
            LoadPreview(MemoryCard.GetSelectedLevelIndex());
        }
    }

    private void LoadPreview(int index)
    {

        float x = 0;

        if (p_currentMapPreview != null)
        {
            x = p_currentMapPreview.transform.position.x; //save the map position before we instantiate another map
            Destroy(p_currentMapPreview);
        }
            

        string sceneName = MemoryCard.GetScene(index);
        p_currentMapPreview = (GameObject)Instantiate(Resources.Load("prefab_" + sceneName));
        p_currentMapPreview.transform.localScale = new Vector3(1, 1, 1) * 0.5f;
        p_currentMapPreview.transform.Rotate(new Vector3(1, 0, 0), -50, Space.World);

        if (x > 0)
        {
            p_currentMapPreview.transform.position = p_mapLeftPosition;
            p_currentMapPreview.transform.Translate(2, 0, 0,Space.World); //translate it one unit over so the if statement for loading a new map wont be triggered
        }
        else
        {
            p_currentMapPreview.transform.position = p_mapRightPosition;
            p_currentMapPreview.transform.Translate(-2, 0, 0, Space.World);
        }

        p_mapGoToThisPosition = p_mapMiddlePosition;

        float bestTime = MemoryCard.LoadHighScore().timeInSeconds;
        p_textString = "level " + index + "\nbest time " + bestTime.ToString("f2") + "s";
    }

    private void AnimateScreenTransitions()
    {
        Vector3 gotoPosition;
        
        if (p_nextScene)
            gotoPosition = p_cameraOutsidePosition;
        else
            gotoPosition = p_cameraInsidePosition;

        m_camera.transform.localPosition = Vector3.SmoothDamp(m_camera.transform.position, gotoPosition, ref p_cameraVelocity, p_cameraSmoothTime);

        if (p_cameraVelocity.magnitude < 1f)
            m_text.text = p_textString;
        else
            m_text.text = "";
    }

    private void AnimateScreen()
    {
        p_currentMapPreview.transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 50);
        ScaleAnimateObject(p_continuePlaneScale, m_continuePlane);
    }

    private void AnimateMapTransition()
    {
        p_currentMapPreview.transform.position = Vector3.SmoothDamp(p_currentMapPreview.transform.position, p_mapGoToThisPosition, ref p_mapVelocity, p_mapSmoothTime);
    }

    private void ScaleAnimateObject(Vector3 scale, GameObject gameObject)
    {
        float sinButtonScale = Mathf.Sin(Time.time * 3.1f);
        gameObject.transform.localScale = scale * (sinButtonScale * sinButtonScale * 0.05f + 1f);
    }

    private void OnClickContinue()
    {
        p_nextScene = true;
    }
}
