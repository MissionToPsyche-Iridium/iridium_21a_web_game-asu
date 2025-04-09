using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float speed = 2f; // Speed of movement
    public float moveAmount = 2f; // How far it moves left to right

    private float startX;
    private bool movingRight = true;

    private void Start()
    {
        startX = transform.position.x;
    }

    private void Update()
    {
        float newX = transform.position.x;

        if (movingRight)
        {
            newX += speed * Time.deltaTime;
            if (newX >= startX + moveAmount)
                movingRight = false;
        }
        else
        {
            newX -= speed * Time.deltaTime;
            if (newX <= startX - moveAmount)
                movingRight = true;
        }

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
