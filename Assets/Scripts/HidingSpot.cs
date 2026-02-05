using UnityEngine;

public class HidingSpot : MonoBehaviour , Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact(PlayerController player)
    {   
        if(player.bIsTryingToHide || player.bIsTryingToReveal) return;
            else if (player.bIsHiding)
            {
                player.bIsInteracting = false;
                player.bIsTryingToReveal = true;
                StartCoroutine(player.RevealCoroutine());
            }
            else
            {
                player.bIsInteracting = true;
                player.bIsTryingToHide = true;
                StartCoroutine(player.HideCoroutine());
            }
        
    }
}
