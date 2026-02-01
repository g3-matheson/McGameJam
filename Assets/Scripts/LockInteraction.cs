using UnityEngine;

public class LockInteraction : MonoBehaviour, Interactable
{
    public void Interact(PlayerController player)
    {
        if (!UIManager.instance.lockPad.gameObject.activeInHierarchy)
        {
            UIManager.instance.lockPad.gameObject.SetActive(true);
            player.bIsInteracting = true;
        }
        else
        {
            UIManager.instance.lockPad.gameObject.SetActive(false);
            player.bIsInteracting = false;
        }
            
    }
}
