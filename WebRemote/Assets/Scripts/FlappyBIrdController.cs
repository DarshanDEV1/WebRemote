using UnityEngine;
using TMPro;
using Google.XR.Cardboard;

public class FlappyBirdController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float doubleJumpForce = 7f;
    public float dashForce = 10f;
    public TMP_Text scoreText;
    public TMP_Text debugText;

    private Rigidbody rb;
    private int score;
    private bool canDoubleJump;
    private bool isPaused;

    private void Awake()
    {
        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        score = 0;
        isPaused = false;
        UpdateScoreText();

        // Subscribe to events
        EventManager.Instance.OnSingleTap += HandleSingleTap;
        EventManager.Instance.OnDoubleTap += HandleDoubleTap;
        EventManager.Instance.OnTripleTap += HandleTripleTap;
        EventManager.Instance.OnSwipeUp += HandleSwipeUp;
        EventManager.Instance.OnSwipeDown += HandleSwipeDown;
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        EventManager.Instance.OnSingleTap -= HandleSingleTap;
        EventManager.Instance.OnDoubleTap -= HandleDoubleTap;
        EventManager.Instance.OnTripleTap -= HandleTripleTap;
        EventManager.Instance.OnSwipeUp -= HandleSwipeUp;
        EventManager.Instance.OnSwipeDown -= HandleSwipeDown;
    }

    void HandleSingleTap()
    {
        if (isPaused) return;

        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        canDoubleJump = true;
        debugText.text = "Single Tap";
    }

    void HandleDoubleTap()
    {
        if (isPaused) return;

        if (canDoubleJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, doubleJumpForce, rb.velocity.z);
            canDoubleJump = false;
            debugText.text = "Double Tap";
        }
    }

    void HandleTripleTap()
    {
        if (isPaused) return;

        rb.velocity = new Vector3(dashForce, rb.velocity.y, rb.velocity.z);
        debugText.text = "Triple Tap";
    }

    void HandleSwipeUp()
    {
        isPaused = !isPaused;
        rb.isKinematic = isPaused;
        debugText.text = isPaused ? "Paused" : "Resumed";
        if (isPaused)
        {
            Time.timeScale = 0.2f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void HandleSwipeDown()
    {
        debugText.text = "Score: " + score;
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ScoreZone"))
        {
            score++;
            UpdateScoreText();
        }
        else if (collision.collider.CompareTag("Obstacle"))
        {
            // Handle game over logic here
            if (score > 0)
            {
                score--;
                scoreText.text = "Score: " + score;
            }
            else
            {
                debugText.text = "Game Over!";
            }
        }
    }
}
