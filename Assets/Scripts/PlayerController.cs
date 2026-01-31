using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float hideSpeed = 2f;

    

    public float hideTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Color PlayerColor;
    
    private Rigidbody2D rb;
    private Interactable currentInteractable;
    private Vector2 movementInput;
    private bool bIsInRangeOfObject;
    public bool bIsTryingToHide;
    public bool bIsHiding;

    private Animator PlayerAnimator;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerAnimator = GetComponent<Animator>();
        
    }

    void Start()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector2 move = movementInput * moveSpeed;
        rb.linearVelocity = move;

        

        if(bIsTryingToHide)
        {
            Hiding();
        }

        UpdateAnimator();    
    }

    void UpdateAnimator()
    {
        PlayerAnimator.SetBool("IsMoving", movementInput.magnitude > 0);
        PlayerAnimator.SetFloat("MoveX", movementInput.x > 0 ? 1 : movementInput.x < 0 ? -1 : 0);
        PlayerAnimator.SetFloat("MoveY", movementInput.y > 0 ? 1 : movementInput.y < 0 ? -1 : 0);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {

        movementInput = context.ReadValue<Vector2>();

    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (bIsInRangeOfObject && context.started && currentInteractable != null)
        {
            currentInteractable?.Interact(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            bIsInRangeOfObject = true;
            currentInteractable = collision.gameObject.TryGetComponent<Interactable>(out Interactable interactable) ? interactable : null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            bIsInRangeOfObject = false;
            currentInteractable = null;
        }
    }

    public void Hiding()
    {

        hideTimer += Time.deltaTime;


        PlayerColor.a = Mathf.Lerp(1f, 0f, hideTimer / hideSpeed);
        spriteRenderer.color = PlayerColor;

        if (hideTimer >= hideSpeed)
        {
            hideTimer = 0;
            bIsHiding = true;
            bIsTryingToHide = false;
                
        }
    }

}
