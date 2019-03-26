using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The FixedJoystick class.
/// Reads input from the joystick.
/// I made manual changes in the Start() method, the rest is from the "Joystick Pack" on the asset store.
/// Changes are between line 18-30.
/// </summary>
public class FixedJoystick : Joystick
{
    Vector2 joystickPosition = Vector2.zero;

    private Camera cam = new Camera();

    void Start()
    {
        //This is my code, the rest is from the asset---------------
        //Transform the joystick manually on the screen

        float size = (Screen.width < Screen.height) ? Screen.width : Screen.height; //Get horizontal or vertical size of the screen.

        background.sizeDelta = new Vector2(size * 0.8f, size * 0.8f); //Make the joystick 80% of the screensize.

        //set the position of the joystick
        float x = background.transform.position.x;
        float y = size / 2f;
        float z = background.transform.position.z;
        background.transform.position = new Vector3(x, y, z);
        //----------------------------------------------------------

        joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickPosition;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}