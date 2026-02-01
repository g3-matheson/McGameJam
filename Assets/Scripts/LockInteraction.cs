using TMPro;
using UnityEngine;

public class LockInteraction : MonoBehaviour, Interactable
{

    private BoxCollider2D boxCollider;
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

    }
    
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
            if (UIManager.instance.lockPad.isUnlocked)
            {
                boxCollider.enabled = false;
            }
        }
            
    }
}
