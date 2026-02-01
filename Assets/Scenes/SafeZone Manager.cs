using UnityEngine;
using System.Collections.Generic;

public class SafeZoneManager : MonoBehaviour
{
    public static SafeZoneManager Instance;

    [System.Serializable]
    public class SafeZone
    {
        public Transform deathPoint;
        public Transform spawnPoint;
    }

    public List<SafeZone> safeZones = new List<SafeZone>();
    public GameObject ratPrefab;
    public float spawnDelay = 2f;
    public int initialRatCount = 3;  //  how many rats

    private Queue<Transform> respawnQueue = new Queue<Transform>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // this is spawn initial rats
        SpawnInitialRats();
    }

    void SpawnInitialRats()
    {
        // This is to spawn initial rats at random safe zones
        for (int i = 0; i < initialRatCount; i++)
        {
            if (safeZones.Count > 0 && ratPrefab != null)
            {
                // Cycle through spawn points
                int zoneIndex = i % safeZones.Count;
                Transform spawnPoint = safeZones[zoneIndex].spawnPoint;

                if (spawnPoint != null)
                {
                    Instantiate(ratPrefab, spawnPoint.position, Quaternion.identity);
                }
            }
        }
    }

    public Transform GetNearestDeathPoint(Vector2 ratPosition)
    {
        Transform nearest = null;
        float nearestDistance = Mathf.Infinity;

        foreach (SafeZone zone in safeZones)
        {
            if (zone.deathPoint != null)
            {
                float distance = Vector2.Distance(ratPosition, zone.deathPoint.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = zone.deathPoint;
                }
            }
        }

        return nearest;
    }

    public void RatReachedDeathPoint(Vector2 deathPosition)
    {
        // Find which safe zone this death point belongs to
        foreach (SafeZone zone in safeZones)
        {
            if (zone.deathPoint != null && Vector2.Distance(deathPosition, zone.deathPoint.position) < 0.5f)
            {

                if (zone.spawnPoint != null)
                {
                    respawnQueue.Enqueue(zone.spawnPoint);
                    Invoke(nameof(SpawnNextRat), spawnDelay);
                }
                break;
            }
        }
    }

    void SpawnNextRat()
    {
        if (respawnQueue.Count > 0 && ratPrefab != null)
        {
            Transform spawnPoint = respawnQueue.Dequeue();
            if (spawnPoint != null)
            {
                Instantiate(ratPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }

    void OnDrawGizmos()
    {
        foreach (SafeZone zone in safeZones)
        {
            if (zone.deathPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(zone.deathPoint.position, 0.5f);
            }

            if (zone.spawnPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(zone.spawnPoint.position, 0.5f);
            }

            if (zone.deathPoint != null && zone.spawnPoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(zone.deathPoint.position, zone.spawnPoint.position);
            }
        }
    }
}