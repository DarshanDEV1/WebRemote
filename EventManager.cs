using System;
using Unity;
using UnityEngine;

public class EventManager
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventManager();
            }
            return _instance;
        }
    }

    private EventManager() { }

    public event Action OnSingleTap;
    public event Action OnDoubleTap;
    public event Action OnTripleTap;
    public event Action OnFourTaps;
    public event Action OnSwipeUp;
    public event Action OnSwipeDown;
    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;
    public event Action<Vector3> OnMouseMove;

    public void TriggerSingleTap() => OnSingleTap?.Invoke();
    public void TriggerDoubleTap() => OnDoubleTap?.Invoke();
    public void TriggerTripleTap() => OnTripleTap?.Invoke();
    public void TriggerFourTaps() => OnFourTaps?.Invoke();
    public void TriggerSwipeUp() => OnSwipeUp?.Invoke();
    public void TriggerSwipeDown() => OnSwipeDown?.Invoke();
    public void TriggerSwipeLeft() => OnSwipeLeft?.Invoke();
    public void TriggerSwipeRight() => OnSwipeRight?.Invoke();
    public void TriggerMouseMove(Vector3 position) => OnMouseMove?.Invoke(position);
}
