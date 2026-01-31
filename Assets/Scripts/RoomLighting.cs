using System.Diagnostics;
using UnityEngine;

public class RoomLighting : MonoBehaviour
{
    public GameObject darkGameObject;

    private BoxCollider2D roomCollider;
    
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
