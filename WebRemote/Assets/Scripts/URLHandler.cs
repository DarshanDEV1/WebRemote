using UnityEngine;
using TMPro;
using System;

public class URLHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text m_DebugText;

    void Awake()
    {
        // Check if the app was opened via a custom URL scheme
        string deepLinkUrl = GetDeepLinkURL();
        if (!string.IsNullOrEmpty(deepLinkUrl))
        {
            string serverUrl = ExtractServerUrl(deepLinkUrl);
            if (!string.IsNullOrEmpty(serverUrl))
            {
                serverUrl = serverUrl.Trim(); // Ensure no leading or trailing spaces
                PlayerPrefs.SetString("baseUrl", serverUrl);
                PlayerPrefs.Save();
                m_DebugText.text = "Connected to: " + serverUrl;
                m_DebugText.color = Color.yellow;
                Debug.Log("<color=green>" + "Connected To: " + serverUrl + "</color>");
                FindObjectOfType<HTTPReceiver>().ConnectServer(serverUrl);
            }
            else
            {
                m_DebugText.text = "Invalid URL format";
                m_DebugText.color = Color.red;
                Debug.Log("<color=red>" + "Invalid URL format" + "</color>");
            }
        }
        else
        {
            m_DebugText.text = "Not Connected to URL";
            m_DebugText.color = Color.red;
            Debug.Log("<color=red>" + "Not Connected To URL " + "</color>");
        }
    }

    string GetDeepLinkURL()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
        string action = intent.Call<string>("getAction");

        if (action == "android.intent.action.VIEW")
        {
            AndroidJavaObject uri = intent.Call<AndroidJavaObject>("getData");
            return uri.Call<string>("toString");
        }
#endif
        return null;
    }

    string ExtractServerUrl(string deepLinkUrl)
    {
        // Example: weblogin://connect?serverUrl=http%3A%2F%2Flocalhost%3A3000
        Uri uri = new Uri(deepLinkUrl);
        string query = uri.Query;
        if (query.StartsWith("?"))
        {
            query = query.Substring(1); // Remove the leading '?'
        }

        var queryParams = query.Split('&');
        foreach (var param in queryParams)
        {
            var keyValue = param.Split('=');
            if (keyValue.Length == 2 && keyValue[0] == "serverUrl")
            {
                return System.Uri.UnescapeDataString(keyValue[1]).Trim();
            }
        }
        return null;
    }
}
