using UnityEngine;
using TMPro;
using System.Linq;

public class LockInteraction : MonoBehaviour, Interactable
{

    public int[] numbers;
    private int index = 0;

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
            
            UIManager.instance.lockPad.gameObject.SetActive(true);
            player.bIsInteracting = true;
        }
    }
}
