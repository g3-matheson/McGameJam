using UnityEngine;

public class RoomLighting : MonoBehaviour
{
    public GameObject darkGameObject;

    private Collider2D roomCollider;

    public GameManager.Room Room;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomCollider = GetComponent<BoxCollider2D>();
        if(Room == GameManager.Room.GirlRoom) return;
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
            GameManager.Instance.PlayerCurrentRoom = Room;
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
