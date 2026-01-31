using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    private Rigidbody2D rb;

    // Read the 2D vector value from the input action
    private Vector2 movementInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 move = movementInput * moveSpeed;
        rb.linearVelocity = move;


    }

    
    public void OnMove(InputAction.CallbackContext context)
    {
        
        movementInput = context.ReadValue<Vector2>();

    }

    public void OnInteract(InputAction.CallbackContext context)
    {

        Debug.Log("Interact pressed");

    }
}
