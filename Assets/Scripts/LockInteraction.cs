using UnityEngine;

public class LockInteraction : MonoBehaviour, Interactable
{

    public int[] numbers;

    void Start()
    {
        
    }

    public void Interact(PlayerController player)
    {
        if (UIManager.instance.lockPad.gameObject.activeInHierarchy)
            {
                UIManager.instance.lockPad.gameObject.SetActive(true);
            
            if (player.bIsInteracting)
            {
                UIManager.instance.lockPad.CheckCombination();
                player.bIsInteracting = false;
            }
        }
        else
        {
            UIManager.instance.lockPad.SetNumber(numbers);
            player.bIsInteracting = true;
        }
    }
}
