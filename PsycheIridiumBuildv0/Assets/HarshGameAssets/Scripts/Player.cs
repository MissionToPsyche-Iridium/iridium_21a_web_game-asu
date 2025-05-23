using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody2D rb;
    private Vector2 playerDirection;
    public bool isCollide = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ProgressBarController.gameOver) return;
        float directionY = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector2(0, directionY).normalized;
    }

    // fixed update for better performance
    private void FixedUpdate()
    {
        if (ProgressBarController.gameOver) return;
        rb.velocity = new Vector2(0, playerDirection.y * playerSpeed);
    }
}
