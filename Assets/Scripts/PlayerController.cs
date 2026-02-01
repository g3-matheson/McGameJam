using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public bool bIsFeeding;

    private Animator PlayerAnimator;
    private InputAction MoveAction;
    private InputAction InteractAction;
    private InputAction ClickAction;
    private InputAction FeedAction;

    public Image FeedingWheel;
    public float FeedingTime;
    private float FeedingTimer;
    private GameObject RatTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        MoveAction = playerInput.actions["Move"];
        InteractAction = playerInput.actions["Interact"];
        ClickAction = playerInput.actions["Click"];
        FeedAction = playerInput.actions["Feed"];
    }

    void Start()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerColor = spriteRenderer.color;
        FeedingTimer = 0f;
    }

    void Update()
    {
        Vector2 move = movementInput * moveSpeed;
        rb.linearVelocity = move;
        
        UpdateAnimator();    
    }

    void FixedUpdate()
    {
        if (bIsFeeding) UpdateFeedMeter();
    }

    void UpdateFeedMeter()
    {
        FeedingTimer += Time.deltaTime;

        float progress = Mathf.Clamp01(FeedingTimer / FeedingTime);
        FeedingWheel.fillAmount = progress;
        if (FeedingTimer > FeedingTime)
        {
            // TODO boost blood    
            // TODO kill rat
            FeedingWheel.fillAmount = 0f;
            bIsFeeding = false;
            FeedingTimer = 0f;
        }
    }

    private void OnEnable()
    {
       MoveAction.Enable();
       InteractAction.Enable();
       ClickAction.Enable();
       FeedAction.Enable();
    }

    private void OnDisable()
    {
       MoveAction.Disable();
       InteractAction.Disable();
       ClickAction.Disable();
       if (!bIsFeeding) FeedAction.Disable();
    }

    void UpdateAnimator()
    {
        PlayerAnimator.SetBool("IsMoving", movementInput.magnitude > 0);

        var moveX = movementInput.x > 0 ? 1 : movementInput.x < 0 ? -1 : 0;
        var moveY = movementInput.y > 0 ? 1 : movementInput.y < 0 ? -1 : 0;
        PlayerAnimator.SetFloat("MoveX", moveX);
        PlayerAnimator.SetFloat("MoveY", moveY);

        if (movementInput.magnitude > 0) 
        {
            PlayerAnimator.SetFloat("LastX", moveX);
            PlayerAnimator.SetFloat("LastY", moveY);
        }

        PlayerAnimator.SetBool("IsFeeding", bIsFeeding);
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

    public void OnFeed(InputAction.CallbackContext context)
    {
        if (RatTarget == null) return;
        else
        {
            // TODO disable RatTarget's movement
        }

        if (context.started && !bIsFeeding)
        {
            bIsFeeding = true; 
            OnDisable();
            PlayerAnimator.SetTrigger("Feed");
            FeedingTimer = 0f;
        }
        else if (context.canceled)
        {
            bIsFeeding = false;
            OnEnable();
            FeedingTimer = 0f;
            FeedingWheel.fillAmount = 0f;
            // TODO re-enable RatTarget's movement
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            bIsInRangeOfObject = true;
            currentInteractable = collision.gameObject.TryGetComponent<Interactable>(out Interactable interactable) ? interactable : null;
        }
        else if (collision.CompareTag("Rat"))
        {
            RatTarget = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            bIsInRangeOfObject = false;
            currentInteractable = null;
        }
        else if (collision.gameObject == RatTarget) RatTarget = null;
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

}
