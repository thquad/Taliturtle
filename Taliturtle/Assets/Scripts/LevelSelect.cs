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

    private bool p_nextScene;
    private bool p_returnScene;

    private void Start()
    {
        p_continuePlaneScale = m_continuePlane.transform.localScale;
        p_levelselectPlaneScale = m_levelselectPlane.transform.localScale;


        m_contButton.onClick.AddListener(OnClickContinue);
        p_nextScene = false;
        p_returnScene = false;
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
                    MemoryCard.AddToSelectedLevel(+1);
                else
                    MemoryCard.AddToSelectedLevel(-1);

                LoadPreview(MemoryCard.GetSelectedLevelIndex());
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
        AnimateScreen();
    }

    private void LoadPreview(int index)
    {

        if (p_currentMapPreview != null)
            Destroy(p_currentMapPreview);

        string sceneName = MemoryCard.GetScene(index);
        p_currentMapPreview = (GameObject)Instantiate(Resources.Load("prefab_" + sceneName));

        p_currentMapPreview.transform.localScale = new Vector3(1, 1, 1) * 0.5f;
        p_currentMapPreview.transform.Rotate(new Vector3(1, 0, 0), -50, Space.World);
        p_currentMapPreview.transform.Translate(new Vector3(0, 1, 5), Space.World);
        
    }

    private void AnimateScreenTransitions()
    {
        Vector3 gotoPosition;
        
        if (p_nextScene)
            gotoPosition = p_cameraOutsidePosition;
        else
            gotoPosition = p_cameraInsidePosition;

        m_camera.transform.localPosition = Vector3.SmoothDamp(m_camera.transform.position, gotoPosition, ref p_cameraVelocity, p_cameraSmoothTime);
    }

    private void AnimateScreen()
    {
        p_currentMapPreview.transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 50);
        ScaleAnimateObject(p_continuePlaneScale, m_continuePlane);
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
