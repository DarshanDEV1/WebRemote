using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class HttpReceiver : MonoBehaviour
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
                    transform.position = newPos;
                }
            }
        }
        else
        {
            Debug.Log("Gesture Received: " + data);
            // Handle other signals using a switch case
            switch (data)
            {
                case "single_tap":
                    Debug.Log("Single Tap");
                    m_DebugText.text = "Single Tap";
                    break;
                case "double_tap":
                    Debug.Log("Double Tap");
                    m_DebugText.text = "Double Tap";
                    break;
                case "triple_tap":
                    Debug.Log("Triple Tap");
                    m_DebugText.text = "Triple Tap";
                    break;
                case "four_taps":
                    Debug.Log("Four Taps");
                    m_DebugText.text = "Four Taps";
                    break;
                case "swipe_up":
                    Debug.Log("Swipe Up");
                    m_DebugText.text = "Swipe Up";
                    break;
                case "swipe_down":
                    Debug.Log("Swipe Down");
                    m_DebugText.text = "Swipe Down";
                    break;
                case "swipe_left":
                    Debug.Log("Swipe Left");
                    m_DebugText.text = "Swipe Left";
                    break;
                case "swipe_right":
                    Debug.Log("Swipe Right");
                    m_DebugText.text = "Swipe Right";
                    break;
                case "mouse_move":
                    Debug.Log("Mouse Move");
                    m_DebugText.text = "Mouse Move";
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
