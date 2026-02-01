using UnityEngine;

public class DeathPoint : MonoBehaviour
{
    private CircleCollider2D deathCollider;

    void Awake()
    {
        deathCollider = GetComponent<CircleCollider2D>();
        if (deathCollider == null)
        {
            deathCollider = gameObject.AddComponent<CircleCollider2D>();
        }
        deathCollider.isTrigger = true;
        deathCollider.radius = 0.5f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if a rat entered
        RatMouvement rat = other.GetComponent<RatMouvement>();
        if (rat != null)
        {
            // Tell the manager a rat reached this death point
            if (SafeZoneManager.Instance != null)
            {
                SafeZoneManager.Instance.RatReachedDeathPoint(transform.position);
            }

            // Destroy the rat
            Destroy(other.gameObject);
        }
    }
}