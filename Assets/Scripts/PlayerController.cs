using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public float actionTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Color PlayerColor;
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Interactable currentInteractable;
    private Vector2 movementInput;

    private AudioManager audioManager;

    private bool bIsInRangeOfObject;
    public bool bIsTryingToHide;
    public bool bIsTryingToReveal;
    public bool bIsHiding;
    public bool bIsFeeding;
    public bool bIsInteracting = false;
    public bool bIsDead = false;
    public bool bHasAmulet = false;

    public Animator PlayerAnimator;
    public InputAction MoveAction;
    private InputAction InteractAction;
    private InputAction ClickAction;
    private InputAction FeedAction;

    public Image FeedingWheel;
    public float FeedingTime;
    private float FeedingTimer;
    

    public float GameOverTimer = 2f;
    public GameObject InteractText;
    public GameObject HideText;
    public BloodSlider bloodSlider;

    public GameObject ExitScreen;

    private HashSet<Collider2D> RatCollisions;
    private CapsuleCollider2D RatCollider;
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

        bloodSlider = GameObject.Find("BloodSlider").GetComponent<BloodSlider>();

        audioManager = FindFirstObjectByType<AudioManager>();
        RatCollider = GetComponent<CapsuleCollider2D>();
        RatCollisions = new();
        bHasAmulet = false;
    }

    void Start()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerColor = spriteRenderer.color;
        FeedingTimer = 0f;
        ExitScreen = GameObject.Find("Exit Panel");
        ExitScreen.SetActive(false);
    }

    void Update()
    {
        if (bIsDead) return;
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
            RatTarget.GetComponent<RatController>().Die();
            bloodSlider.AddBlood(bloodSlider.GetTotalTime() / 3);
            FeedingWheel.fillAmount = 0f;
            bIsFeeding = false;
            FeedingTimer = 0f;
            OnEnable();
        }
    }

    public void OnEnable()
    {
        if (bIsDead) return;
        MoveAction.Enable();
        InteractAction.Enable();
        ClickAction.Enable();
        FeedAction.Enable();
    }

    private void OnDisable()
    {
       MoveAction.Disable();
       if (!bIsInteracting) InteractAction.Disable();
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
        if (bIsInRangeOfObject && context.started && currentInteractable != null && !bIsDead)
        {
            currentInteractable?.Interact(this);
            if (bIsInteracting) OnDisable();
            else OnEnable();
        }
    }

    public void OnFeed(InputAction.CallbackContext context)
    {
        if (context.started && !bIsFeeding)
        {
            List<GameObject> ratsInRange = RatCollisions.Where(c => c.CompareTag("Rat"))
                                                       .Select(c => c.gameObject)
                                                       .ToList();
            RatTarget = ratsInRange.Count > 0 ? ratsInRange[Random.Range(0, ratsInRange.Count-1)] : null;

            
			if (RatTarget == null) return;
            bIsFeeding = true; 

            OnDisable();
            PlayerAnimator.SetTrigger("Feed");
            FeedingTimer = 0f;
            RatTarget.GetComponent<RatController>().Freeze = true;
        }
        else if (bIsFeeding & context.canceled)
        {
            bIsFeeding = false;
            OnEnable();
            FeedingTimer = 0f;
            FeedingWheel.fillAmount = 0f;
            if (RatTarget != null) RatTarget.GetComponent<RatController>().Freeze = false;
            RatTarget = null; 
        }
    }

    public void OnShout(InputAction.CallbackContext context)
    {
        HunterAI.Instance.PlayerShout(context);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            bIsInRangeOfObject = true;
            if (collision.gameObject.TryGetComponent<HidingSpot>(out var _))
            {
                HideText.SetActive(true);
            }
            else
            {
                InteractText.SetActive(true);
            }
            currentInteractable = collision.gameObject.TryGetComponent<Interactable>(out Interactable interactable) ? interactable : null;
        }
        else if (collision.CompareTag("Rat"))
        {
            RatCollisions.Add(collision);
            collision.gameObject.GetComponent<RatController>().Fleeing = true;
        }
        else if (collision.CompareTag("Die"))
        {
            if (!bHasAmulet) Death();
            else
            {
                bloodSlider.StopTimer();
                ExitScreen.SetActive(true);
                audioManager.PlayEndingMusic();
                GameManager.Instance.bWin = true;
                OnDisable();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            bIsInRangeOfObject = false;
            HideText.SetActive(false);
            InteractText.SetActive(false);
            currentInteractable = null;
        }
        else if (collision.CompareTag("Rat")) 
        {
            collision.gameObject.GetComponent<RatController>().Fleeing = false;
            RatCollisions.Remove(collision);
        }
    }

    public IEnumerator HideCoroutine()
    {
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
                HideText.SetActive(false);
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
                if (bIsInRangeOfObject) HideText.SetActive(true);
                bIsHiding = false;
                bIsTryingToReveal = false;
                break;
            }
        }
    }

    public IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(GameOverTimer);
        GameManager.Instance.ReloadScene();
    }


    public void Death()
    {
        PlayerAnimator.SetTrigger("Die");
        bIsDead = true;
        OnDisable();
        StartCoroutine(GameOverCoroutine());
        rb.linearVelocity = Vector2.zero;
    }

}
