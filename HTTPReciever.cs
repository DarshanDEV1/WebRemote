using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class HTTPReceiver : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_UrlInputField;
    [SerializeField] private Button m_Submit;
    [SerializeField] private TMP_Text m_DebugText;
    [SerializeField] private string serverUrl = "http://localhost:3000"; // Replace with your ngrok URL
    private float requestInterval = 1.0f;  // Interval in seconds
    private string url = "http://localhost:3000/get-latest-signal";

    private void Awake()
    {
        url = serverUrl + "/get-latest-signal";
    }

    void Start()
    {
        m_Submit.onClick.AddListener(() =>
        {
            StopAllCoroutines();
            url = m_UrlInputField.text + "/get-latest-signal";
            StartCoroutine(GetLatestSignal(url));
        });
    }

    IEnumerator GetLatestSignal(string uri)
    {
        while (true)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(uri))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + www.error);
                }
                else
                {
                    string jsonResponse = www.downloadHandler.text;
                    SignalResponse response = JsonUtility.FromJson<SignalResponse>(jsonResponse);
                    if (!string.IsNullOrEmpty(response.signal))
                    {
                        Debug.Log("Received signal: " + response.signal);
                        ProcessReceivedData(response.signal);
                    }
                }
            }
            yield return new WaitForSeconds(requestInterval);
        }
    }

    void ProcessReceivedData(string data)
    {
        if (data.StartsWith("mouse_move:"))
        {
            string[] coordinates = data.Substring(11).Split(',');
            if (coordinates.Length == 2)
            {
                float x, y;
                if (float.TryParse(coordinates[0], out x) && float.TryParse(coordinates[1], out y))
                {
                    Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, Camera.main.nearClipPlane));
                    EventManager.Instance.TriggerMouseMove(newPos);
                }
            }
        }
        else
        {
            Debug.Log("Gesture Received: " + data);
            switch (data)
            {
                case "single_tap":
                    EventManager.Instance.TriggerSingleTap();
                    break;
                case "double_tap":
                    EventManager.Instance.TriggerDoubleTap();
                    break;
                case "triple_tap":
                    EventManager.Instance.TriggerTripleTap();
                    break;
                case "four_taps":
                    EventManager.Instance.TriggerFourTaps();
                    break;
                case "swipe_up":
                    EventManager.Instance.TriggerSwipeUp();
                    break;
                case "swipe_down":
                    EventManager.Instance.TriggerSwipeDown();
                    break;
                case "swipe_left":
                    EventManager.Instance.TriggerSwipeLeft();
                    break;
                case "swipe_right":
                    EventManager.Instance.TriggerSwipeRight();
                    break;
                default:
                    Debug.LogWarning("Unhandled message: " + data);
                    break;
            }
        }
    }

    [System.Serializable]
    public class SignalResponse
    {
        public string signal;
    }
}
