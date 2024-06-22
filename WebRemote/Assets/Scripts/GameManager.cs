using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text m_DebugText;

    private void OnEnable()
    {
        EventManager.Instance.OnSingleTap += HandleSingleTap;
        EventManager.Instance.OnDoubleTap += HandleDoubleTap;
        EventManager.Instance.OnTripleTap += HandleTripleTap;
        EventManager.Instance.OnFourTaps += HandleFourTaps;
        EventManager.Instance.OnSwipeUp += HandleSwipeUp;
        EventManager.Instance.OnSwipeDown += HandleSwipeDown;
        EventManager.Instance.OnSwipeLeft += HandleSwipeLeft;
        EventManager.Instance.OnSwipeRight += HandleSwipeRight;
        EventManager.Instance.OnMouseMove += HandleMouseMove;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnSingleTap -= HandleSingleTap;
        EventManager.Instance.OnDoubleTap -= HandleDoubleTap;
        EventManager.Instance.OnTripleTap -= HandleTripleTap;
        EventManager.Instance.OnFourTaps -= HandleFourTaps;
        EventManager.Instance.OnSwipeUp -= HandleSwipeUp;
        EventManager.Instance.OnSwipeDown -= HandleSwipeDown;
        EventManager.Instance.OnSwipeLeft -= HandleSwipeLeft;
        EventManager.Instance.OnSwipeRight -= HandleSwipeRight;
        EventManager.Instance.OnMouseMove -= HandleMouseMove;
    }

    private void HandleSingleTap()
    {
        Debug.Log("Single Tap");
        m_DebugText.text = "Single Tap";
        // Add more functionality here
    }

    private void HandleDoubleTap()
    {
        Debug.Log("Double Tap");
        m_DebugText.text = "Double Tap";
        // Add more functionality here
    }

    private void HandleTripleTap()
    {
        Debug.Log("Triple Tap");
        m_DebugText.text = "Triple Tap";
        // Add more functionality here
    }

    private void HandleFourTaps()
    {
        Debug.Log("Four Taps");
        m_DebugText.text = "Four Taps";
        // Add more functionality here
    }

    private void HandleSwipeUp()
    {
        Debug.Log("Swipe Up");
        m_DebugText.text = "Swipe Up";
        // Add more functionality here
    }

    private void HandleSwipeDown()
    {
        Debug.Log("Swipe Down");
        m_DebugText.text = "Swipe Down";
        // Add more functionality here
    }

    private void HandleSwipeLeft()
    {
        Debug.Log("Swipe Left");
        m_DebugText.text = "Swipe Left";
        // Add more functionality here
    }

    private void HandleSwipeRight()
    {
        Debug.Log("Swipe Right");
        m_DebugText.text = "Swipe Right";
        // Add more functionality here
    }

    private void HandleMouseMove(Vector3 position)
    {
        Debug.Log("Mouse Move to position: " + position);
        m_DebugText.text = "Mouse Move";
        // Add more functionality here
    }
}
