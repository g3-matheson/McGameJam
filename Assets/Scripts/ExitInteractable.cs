using UnityEngine;

public class ExitInteractable : MonoBehaviour, Interactable
{
    
    public BoxCollider2D triggerCollider;

    void Awake()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
    }

    
    public void Interact(PlayerController player)
    {
        if (player.bHasAmulet)
        {
            Debug.Log("You Escaped!");
            // start escape logic here.
        }
        
        else
        {
            player.Death();
        }
    }
    
}
