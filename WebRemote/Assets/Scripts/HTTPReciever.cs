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
    [SerializeField] private GameObject m_Canvas;
    [SerializeField] private GameObject m_GameCanvas;
    private float requestInterval = 1.0f;  // Interval in seconds
    private string m_url;

    private void Awake()
    {
        m_Canvas.SetActive(true);
        m_GameCanvas.SetActive(false);
    }

    void Start()
    {
        m_url = PlayerPrefs.GetString("serverUrl", "https://d9a7-117-254-139-32.ngrok-free.app");
        //m_UrlInputField.text = PlayerPrefs.GetString("serverUrl", "https://d9a7-117-254-139-32.ngrok-free.app");
        //m_Submit.onClick.AddListener(ConnectServer);
        //ConnectServer();
    }

    public void ConnectServer(string m_Base_URL)
    {
        string baseUrl = PlayerPrefs.GetString("serverUrl", m_Base_URL);
        if (!string.IsNullOrEmpty(baseUrl))
        {
            m_url = baseUrl + "/get-latest-signal";
            PlayerPrefs.SetString("serverUrl", baseUrl);
            PlayerPrefs.Save();

            StopAllCoroutines();
            StartCoroutine(GetLatestSignal(m_url));

            m_Canvas.SetActive(false);
            m_GameCanvas.SetActive(true);
        }
        else
        {
            m_DebugText.text = "Please enter a valid URL.";
        }
    }

    IEnumerator GetLatestSignal(string uri)
    {
        m_DebugText.text = "Connected To : " + uri;
        m_DebugText.color = Color.green;
        while (true)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(uri))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + www.error);
                    m_DebugText.text = "Error: " + www.error;
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
