using TMPro;
using UnityEngine;

public class LockInteraction : MonoBehaviour, Interactable
{
    UIManager uIManager;
    private BoxCollider2D boxCollider;
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        uIManager = FindFirstObjectByType<UIManager>();
    }
    
    public void Interact(PlayerController player)
    {
        if (!uIManager.lockPad.gameObject.activeInHierarchy)
        {
            uIManager.lockPad.gameObject.SetActive(true);
            player.bIsInteracting = true;
        }
        else
        {
            uIManager.lockPad.gameObject.SetActive(false);
            player.bIsInteracting = false;
            if (uIManager.lockPad.isUnlocked)
            {
                boxCollider.enabled = false;
            }
        }
            
    }
}
