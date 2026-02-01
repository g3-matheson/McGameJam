using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        float moveX = Input.GetAxisRaw("Horizontal"); // This is for A/D or Left/Right arrow keys
        float moveY = Input.GetAxisRaw("Vertical");   //This is for W/S or Up/Down arrow keys


        Vector2 movement = new Vector2(moveX, moveY).normalized;

        // Apply movement
        rb.linearVelocity = movement * moveSpeed;
    }
}