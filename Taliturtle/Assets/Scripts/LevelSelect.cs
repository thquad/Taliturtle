using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The LevelSelect class.
/// Used in the level select screen to handle input and animate elements.
/// </summary>
public class LevelSelect : MonoBehaviour
{
    public Button m_contButton;
    public GameObject m_camera;
    public GameObject m_continuePlane;
    public GameObject m_levelselectPlane;
    public Text m_text;

    //scale of the "buttons"
    private Vector3 p_continuePlaneScale;
    private Vector3 p_levelselectPlaneScale;

    //variables for touch controls
    private Vector2 p_startPos;
    private Vector2 p_direction;
    private float p_triggerDistance;

    //currently displayed map values
    private GameObject p_currentMapPreview;
    private float p_lastLevelChange;
    private Vector3 p_mapMiddlePosition = new Vector3(0, 1, 5);
    private Vector3 p_mapLeftPosition = new Vector3(-20, 1, 5);
    private Vector3 p_mapRightPosition = new Vector3(20, 1, 5);
    private Vector3 p_mapGoToThisPosition = new Vector3(0, 1, 5);
    private float p_mapSmoothTime = 0.1f;
    private Vector3 p_mapVelocity = new Vector3();

    //camera
    private Vector3 p_cameraVelocity;
    private Vector3 p_cameraInsidePosition;
    private Vector3 p_cameraOutsidePosition;
    private float p_cameraSmoothTime;

    //bools for changing scenes
    private bool p_nextScene = false;
    private bool p_returnScene = false;

    //information for the map
    private string p_textString = "";

    // Start is called before the first frame update
    private void Start()
    {
        p_continuePlaneScale = m_continuePlane.transform.localScale;
        p_levelselectPlaneScale = m_levelselectPlane.transform.localScale;

        m_contButton.onClick.AddListener(OnClickContinue);

        //move the camera outside so it can swipe in
        p_cameraInsidePosition = m_camera.transform.position;
        m_camera.transform.Translate(-20, 0, 0);
        p_cameraOutsidePosition = m_camera.transform.position;
        p_cameraSmoothTime = 0.3f;

        p_lastLevelChange = 1;
        p_triggerDistance = Screen.width*0.1f; //if 10% of the screen has been touchmoved

        LoadPreview(MemoryCard.GetSelectedLevelIndex());
    }

    // Update is called once per frame
    private void Update()
    {
        //return key pressed?
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            p_nextScene = true;
            p_returnScene = true;
        }

        //check touch controls
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //save the touch and swipe positions
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    p_startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    p_direction = touch.position - p_startPos;
                    break;
                case TouchPhase.Ended:
                    p_direction.x = 0;
                    p_direction.y = 0;
                    break;
            }
        }

        //if the player swiped for a certain distance, start to load new map
        if (Mathf.Abs(p_direction.x) > p_triggerDistance)
        {
            //dont change map to frequently
            if (Time.time - p_lastLevelChange > 0.3f)
            {
                p_lastLevelChange = Time.time;

                //change to next or previous map and set animatin accordingly
                if (p_direction.x < 0)
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

        //change scene once the camera finished animation
        if(p_nextScene && m_camera.transform.position.x < -19)
        {
            if (p_returnScene)
                MemoryCard.LoadMenu();
            else
                MemoryCard.LoadSelectedLevel();
        }

        //animate
        AnimateScreenTransitions();
        AnimateMapTransition();
        AnimateScreen();

        //if the map has finished animation, load a new map
        if(p_currentMapPreview.transform.position.x < -19 || p_currentMapPreview.transform.position.x > 19)
        {
            LoadPreview(MemoryCard.GetSelectedLevelIndex()); //load currently selected level
        }
    }

    /// <summary>
    /// Loads a new map, updates the gui and initialises animation.
    /// </summary>
    /// <param name="index">Which map to load.</param>
    private void LoadPreview(int index)
    {

        float x = 0;

        //unload current map
        if (p_currentMapPreview != null)
        {
            x = p_currentMapPreview.transform.position.x; //save the map position before we instantiate another map
            Destroy(p_currentMapPreview);
        }
            
        //load new map and initialise
        string sceneName = MemoryCard.GetScene(index);
        p_currentMapPreview = (GameObject)Instantiate(Resources.Load("prefab_" + sceneName));
        p_currentMapPreview.transform.localScale = new Vector3(1, 1, 1) * 0.5f;
        p_currentMapPreview.transform.Rotate(new Vector3(1, 0, 0), -50, Space.World);

        //spawn the new map on the left or right side of the screen dependent on last mapanimation
        if (x > 0)
        {
            p_currentMapPreview.transform.position = p_mapLeftPosition;
            p_currentMapPreview.transform.Translate(2, 0, 0,Space.World); //translate it a little bit so the if statement for loading a new map wont be triggered
        }
        else
        {
            p_currentMapPreview.transform.position = p_mapRightPosition;
            p_currentMapPreview.transform.Translate(-2, 0, 0, Space.World);
        }

        p_mapGoToThisPosition = p_mapMiddlePosition; //new destination for the map is the center

        //update the gui, highscore
        float bestTime = MemoryCard.LoadHighScore().timeInSeconds;

        if(bestTime!=0)
            p_textString = "level " + (index+1) + "\nbest time " + bestTime.ToString("f2") + "s";
        else
            p_textString = "level " + (index + 1) + "\nbest time --.--s";
    }

    /// <summary>
    /// Animates the camera and gui between screen transitions.
    /// </summary>
    private void AnimateScreenTransitions()
    {
        Vector3 gotoPosition;
        
        //set the camera destination according to state
        if (p_nextScene)
            gotoPosition = p_cameraOutsidePosition;
        else
            gotoPosition = p_cameraInsidePosition;

        //animate the camera
        m_camera.transform.localPosition = Vector3.SmoothDamp(m_camera.transform.position, gotoPosition, ref p_cameraVelocity, p_cameraSmoothTime);

        //show text dependent on camera position
        if (p_cameraVelocity.magnitude < 1f)
            m_text.text = p_textString;
        else
            m_text.text = "";
    }

    /// <summary>
    /// Animate elements on screen in a pleasing manner.
    /// </summary>
    private void AnimateScreen()
    {
        //rotate map slowly
        p_currentMapPreview.transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 50);

        //make the buttons wobble
        ScaleAnimateObject(p_continuePlaneScale, m_continuePlane);
    }

    private void AnimateMapTransition()
    {
        p_currentMapPreview.transform.position = Vector3.SmoothDamp(p_currentMapPreview.transform.position, p_mapGoToThisPosition, ref p_mapVelocity, p_mapSmoothTime);
    }

    /// <summary>
    /// Animates an object to wobble.
    /// </summary>
    /// <param name="scale">Original scale of the object.</param>
    /// <param name="gameObject">Object to animate.</param>
    private void ScaleAnimateObject(Vector3 scale, GameObject gameObject)
    {
        float sinButtonScale = Mathf.Sin(Time.time * 3.1f);
        gameObject.transform.localScale = scale * (sinButtonScale * sinButtonScale * 0.05f + 1f);
    }

    /// <summary>
    /// Button event to change to new screen.
    /// </summary>
    private void OnClickContinue()
    {
        p_nextScene = true;
    }
}
