using System;
using System.Text;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using TMPro;

public class UDPReceiver : MonoBehaviour
{
    [SerializeField] private TMP_Text m_DebugText;

    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;
    private int port = 41234;

    void Start()
    {
        udpClient = new UdpClient(port);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
        Debug.Log("UDP Receiver started on port " + port);
        BeginReceive();
    }

    void BeginReceive()
    {
        udpClient.BeginReceive(new AsyncCallback(OnReceive), null);
    }

    void OnReceive(IAsyncResult ar)
    {
        byte[] receivedBytes = udpClient.EndReceive(ar, ref remoteEndPoint);
        string receivedText = Encoding.ASCII.GetString(receivedBytes);
        Debug.Log("Received: " + receivedText);
        ProcessReceivedData(receivedText);
        BeginReceive();
    }

    void ProcessReceivedData(string data)
    {
        // Handle received data
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
            // Add more logic for other signals like single_tap, double_tap, etc.
            switch (data)
            {
                case "single_tap":
                    // Handle single tap
                    Debug.Log("Single Tap");
                    m_DebugText.text = "Single Tap";
                    break;
                case "double_tap":
                    // Handle double tap
                    Debug.Log("Double Tap");
                    m_DebugText.text = "Double Tap";
                    break;
                case "triple_tap":
                    // Handle triple tap
                    Debug.Log("Triple Tap");
                    m_DebugText.text = "Triple Tap";
                    break;
                case "four_taps":
                    // Handle four taps
                    Debug.Log("Four Taps");
                    m_DebugText.text = "Four Taps";
                    break;
                case "swipe_up":
                    // Handle swipe up
                    Debug.Log("Swipe Up");
                    m_DebugText.text = "Swipe Up";
                    break;
                case "swipe_down":
                    // Handle swipe down
                    Debug.Log("Swipe Down");
                    m_DebugText.text = "Swipe Down";
                    break;
                case "swipe_left":
                    // Handle swipe left
                    Debug.Log("Swipe Left");
                    m_DebugText.text = "Swipe Left";
                    break;
                case "swipe_right":
                    // Handle swipe right
                    Debug.Log("Swipe Right");
                    m_DebugText.text = "Swipe Right";
                    break;
                case "mouse_move":
                    // Handle mouse move
                    Debug.Log("Mouse Move");
                    m_DebugText.text = "Mouse Move";
                    break;
                default:
                    Debug.LogWarning("Unhandled message: " + data);
                    break;
            }
        }

        void OnApplicationQuit()
        {
            udpClient.Close();
        }
    }
}