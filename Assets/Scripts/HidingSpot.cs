using UnityEngine;

public class HidingSpot : MonoBehaviour , Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact(PlayerController player)
    {   
        
        if (player.bIsTryingToHide) return;
        player.bIsTryingToHide = true;
        
    }
}
