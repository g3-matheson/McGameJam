using UnityEngine;

public class DollInteraction : MonoBehaviour, Interactable
{
    public string[] lines;
    public void Interact(PlayerController player)
    {
        if (UIManager.instance.dialogueBox.gameObject.activeInHierarchy)
        {
            if (UIManager.instance.dialogueBox.isScrolling)
            {
                UIManager.instance.dialogueBox.CompleteLine();
            }
            else
            {
                UIManager.instance.dialogueBox.NextLine();
            }
        }
        UIManager.instance.dialogueBox.SetLines(lines);
    }
}
