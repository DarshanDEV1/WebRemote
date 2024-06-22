using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 5.0f;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.x < -10)
        {
            Destroy(gameObject);
        }
    }
}
