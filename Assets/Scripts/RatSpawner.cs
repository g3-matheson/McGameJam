using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    public GameManager.Room Room;
    public GameObject Rat;

    private int MaxRats;

    void Start()
    {
        MaxRats = GameManager.Instance.MaxRats[Room]; 
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.CurrentRats[Room] < MaxRats)
        {
            var obj = Instantiate(Rat, transform.position, Quaternion.identity);
            var rc = obj.GetComponent<RatController>();
            rc.Room = Room;
            GameManager.Instance.CurrentRats[Room]++;
        }
    }
}