using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SafeZoneManager : MonoBehaviour
{

    public List<GameObject> spawnPoints;
    public GameObject ratPrefab;
    [SerializeField] private float respawnDelay = 5f;
    [SerializeField] private int maxRats = 3; // maximum number of rats allowed

    private int currentRatCount = 0;

    void Start()
    {
        StartCoroutine(SpawnRatsRoutine());
    }

    IEnumerator SpawnRatsRoutine()
    {
        while (true)
        {
            if (currentRatCount < maxRats)
            {
                SpawnRat();
            }
            yield return new WaitForSeconds(respawnDelay);
        }
    }

    void SpawnRat()
    {
        if (spawnPoints.Count == 0) return;
        int randomIndex = Random.Range(0, spawnPoints.Count);
        GameObject rat = Instantiate(ratPrefab, spawnPoints[randomIndex].transform.position, Quaternion.identity);
        rat.GetComponent<RatMouvement>().owner = this;
        currentRatCount++;
    }

    public void KillRat(RatMouvement rat)
    {
        Destroy(rat.gameObject);
        currentRatCount = Mathf.Max(0, currentRatCount - 1);
    }

}