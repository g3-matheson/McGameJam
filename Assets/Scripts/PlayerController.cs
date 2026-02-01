using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public float actionTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Color PlayerColor;
    
    private Rigidbody2D rb;

    public PlayerInput playerInput;
    private Interactable currentInteractable;
    private Vector2 movementInput;
    private bool bIsInRangeOfObject;
    public bool bIsTryingToHide;
    public bool bIsTryingToReveal;
    public bool bIsHiding;

    private Animator PlayerAnimator;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        
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

    public IEnumerator HideCoroutine()
    {
        playerInput.actions["Move"].Disable();
        while (PlayerColor.a > 0f)
        {
            yield return new WaitForSeconds(0.1f);
            PlayerColor.a -= 0.1f;
            spriteRenderer.color = PlayerColor;
            if(PlayerColor.a <= 0f)
            {
                PlayerColor.a = 0f;
                spriteRenderer.color = PlayerColor;
                bIsHiding = true;
                bIsTryingToHide = false;
                break;
            }
            
        }
    }

    public IEnumerator RevealCoroutine()
    {
        while (PlayerColor.a < 1f)
        {
            yield return new WaitForSeconds(0.1f);
            PlayerColor.a += 0.1f;
            spriteRenderer.color = PlayerColor;
            if (PlayerColor.a >= 1f)
            {
                PlayerColor.a = 1f;
                spriteRenderer.color = PlayerColor;
                bIsHiding = false;
                bIsTryingToReveal = false;
                playerInput.actions["Move"].Enable();
                break;
            }
        }
    }

    public void Death()
    {
        OnDisable();
    }
    
    private void OnEnable()
    {
        playerInput.actions["Move"].Enable();
        playerInput.actions["Interact"].Enable();
        playerInput.actions["Click"].Enable();
    }

    private void OnDisable()
    {
        playerInput.actions["Move"].Disable();
        playerInput.actions["Interact"].Disable();
        playerInput.actions["Click"].Disable();
    }

}
