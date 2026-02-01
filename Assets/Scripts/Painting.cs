using UnityEngine;

public class Painting : MonoBehaviour, Interactable
{
    private bool isVisible = false;
    public void Interact(PlayerController player)
    {
        
        if (!isVisible)
        {
            UIManager.instance.paintingImage.SetActive(true);
            isVisible = true;
            player.playerInput.actions["Move"].Disable();
        }
        else
        {
            UIManager.instance.paintingImage.SetActive(false);
            isVisible = false;
            player.playerInput.actions["Move"].Enable();
        }
    }
}
