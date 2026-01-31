using UnityEngine;
using UnityEngine.InputSystem;

public class MiniPlayerController : MonoBehaviour
{
    Vector2 moveInput;

    void Awake()
    {
        
    }

    void Update()
    {
       if (moveInput.magnitude > 0) transform.position += new Vector3(moveInput.x, moveInput.y, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
