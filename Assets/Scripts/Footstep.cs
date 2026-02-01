using UnityEngine;
using System.Collections;

public class Footstep : MonoBehaviour
{
    public float SelfDestructTimer = 2f;

    void Awake()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(SelfDestructTimer);
        Destroy(gameObject);
    }

}
