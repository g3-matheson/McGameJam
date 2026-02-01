using UnityEngine;

public class RoomLighting : MonoBehaviour
{
    public GameObject darkGameObject;

    private BoxCollider2D roomCollider;

    public GameManager.Room Room;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomCollider = GetComponent<BoxCollider2D>();
        darkGameObject.SetActive(false);
    }

    public void TurnOnLight()
    {
        darkGameObject.SetActive(true);
    }

    public void TurnOffLight()
    {
        darkGameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            TurnOnLight();
            //GameManager.Instance.PlayerCurrentRoom = Room;
        }

        if (other.CompareTag("Hunter") && HunterAI.Instance.ChasingPlayer)
        {
            HunterAI.Instance.CurrentRoom = Room;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            TurnOffLight();
        }
    }
}
