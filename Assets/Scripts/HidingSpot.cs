using UnityEngine;

public class HidingSpot : MonoBehaviour , Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact(PlayerController player)
    {   
        
        player.bIsTryingToHide = !player.bIsTryingToHide;
        if (player.hideTimer == 0f)
        {
            player.bInputEnabled = false;
            player.bIsHiding = true;
        }
        

    }
}
