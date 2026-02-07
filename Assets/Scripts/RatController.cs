using UnityEngine;
using System.Collections;

public class RatController : MonoBehaviour
{
    public GameManager.Room Room;
    public Animator RatAnimator;
    public Rigidbody2D rb;

    public CircleCollider2D ratCollider;

    public float ChangeDirectionTimer;
    private float _timer;
    public float Speed;

    public bool Fleeing;
    private bool Dead;
    public bool Freeze;

    public AudioSource audioSource;

    public AudioClip fleeSound;

    void Awake()
    {
        RatAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _timer = 0;
        Fleeing = false;
        Dead = false;
        Freeze = false;
        ratCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
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
            rb.linearVelocity = Freeze ? Vector2.zero : dir * 2 * Speed;
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(fleeSound);
            } 
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
        rb.Sleep();
        ratCollider.enabled = false;
        StartCoroutine(DeathTimer());
    }

    //Everytime the rat collides with something, if it was fleeing, Destroy the game object,
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Fleeing && !Dead &&
        	!collision.gameObject.CompareTag("Rat")
         && !collision.gameObject.CompareTag("Player")
         && !collision.gameObject.CompareTag("Hunter")
         && collision.gameObject.name != "Horse")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}