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

	void FixedUpdate()
	{
		if (uIManager.lockPad.gameObject.activeInHierarchy && uIManager.lockPad.isUnlocked)
        {
            uIManager.lockPad.gameObject.SetActive(false);
            HunterAI.Instance.playerController.bIsInteracting = false;
            HunterAI.Instance.playerController.OnEnable();
            boxCollider.enabled = false;
            gameObject.SetActive(false);
        }
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
                gameObject.SetActive(false);
            }
        }
            
    }
}
