using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RatMouvement : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    public float moveRadius = 5f;
    public Vector2 circleCenter = Vector2.zero;
    public float targetReachedDistance = 0.2f;

    [Header("Flee")]
    public float fleeSpeed = 8f;
    public float fleeDistance = 6f;

    private Vector2 targetPosition;
    private bool isFleeing;

    private Transform playerTransform;
    private Transform nearestDeathPoint;

    public SafeZoneManager owner;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Start()
    {
        SetNewRandomTarget();
    }

    void FixedUpdate()
    {
        if (isFleeing && playerTransform != null)
        {
            FleeBehaviour();
        }
        else
        {
            WanderBehaviour();
        }
    }

    void FleeBehaviour()
    {
        Vector2 direction = (Vector2)transform.position - (
        (Vector2)playerTransform.position).normalized;

        rb.linearVelocity = direction * fleeSpeed;
        RotateTowards(direction);

        if (Vector2.Distance(transform.position, playerTransform.position) > fleeDistance)
        {
            isFleeing = false;
            playerTransform = null;
            SetNewRandomTarget();
        }
    }

    void WanderBehaviour()
    {
        Vector2 direction =
            (targetPosition - (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
        RotateTowards(direction);

        if (Vector2.Distance(transform.position, targetPosition) <
            targetReachedDistance)
        {
            SetNewRandomTarget();
        }
    }

    void RotateTowards(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation =
            Quaternion.Euler(0, 0, angle - 90f);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !isFleeing)
        {
            isFleeing = true;
            playerTransform = other.transform;
        }
        else if (other.gameObject.tag != "Player" && isFleeing)
        {
            owner.KillRat(this);
        }
    }

    void SetNewRandomTarget()
    {
        float angle = Random.value * Mathf.PI * 2f;
        float distance = Mathf.Sqrt(Random.value) * moveRadius;

        Vector2 offset = new Vector2(
            Mathf.Cos(angle),
            Mathf.Sin(angle)
        ) * distance;

        targetPosition = circleCenter + offset;
    }


}
