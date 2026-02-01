using UnityEngine;

public class RatController : MonoBehaviour
{
    public GameManager.Room Room;
    public Animator RatAnimator;
    public Rigidbody2D rb;

    public float ChangeDirectionTimer;
    private float _timer;
    public float Speed;

    public bool Fleeing;
    private bool Dead;
    public bool Freeze;

    void Awake()
    {
        RatAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _timer = 0;
        Fleeing = false;
        Dead = false;
        Freeze = false;
    }

    void Update()
    {
        if(Dead) return;
    }

    void FixedUpdate()
    {
        if(Dead) return;
        if (Fleeing)
        {
            var dir = (transform.position - HunterAI.Instance.Player.transform.position).normalized;
            rb.linearVelocity = Freeze ? Vector2.zero : dir * Speed; 
        }
        else
        {
            _timer += Time.deltaTime; 
            if (_timer > ChangeDirectionTimer)
            {
                PickNewDirection();
                _timer = 0f;
            }
        }
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        var movementInput = rb.linearVelocity.normalized;
        RatAnimator.SetBool("IsMoving", movementInput.magnitude > 0);
        var moveX = movementInput.x > 0 ? 1 : movementInput.x < 0 ? -1 : 0;
        var moveY = movementInput.y > 0 ? 1 : movementInput.y < 0 ? -1 : 0;
        RatAnimator.SetFloat("MoveX", moveX);
        RatAnimator.SetFloat("MoveY", moveY);
    }

    void PickNewDirection()
    {
        var x = (Random.value * 2) - 1;
        var y = (Random.value * 2) - 1;
        rb.linearVelocity = Freeze ? Vector2.zero : (new Vector2(x,y).normalized) * Speed;
    }

    public void Die()
    {
        Dead = true;
        RatAnimator.SetTrigger("Die");
        GameManager.Instance.CurrentRats[Room]--;
    }
}