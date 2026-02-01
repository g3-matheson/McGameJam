using UnityEngine;

public class DollInteraction : MonoBehaviour, Interactable
{
    public string[] lines;
    public void Interact(PlayerController player)
    {
        
        if (UIManager.instance.dialogueBox.gameObject.activeInHierarchy)
        {
            UIManager.instance.dialogueBox.gameObject.SetActive(true);
            if (UIManager.instance.dialogueBox.isScrolling)
            {
                UIManager.instance.dialogueBox.CompleteLine();
            }
            else if (UIManager.instance.dialogueBox.index >= lines.Length - 1)
            {
                UIManager.instance.dialogueBox.NextLine();
                player.bIsInteracting = false;
            }
            
            else
            {
                UIManager.instance.dialogueBox.NextLine();
            }
        }
        else
        {
            UIManager.instance.dialogueBox.SetLines(lines);
            player.bIsInteracting = true;
        }
        
    }
}
