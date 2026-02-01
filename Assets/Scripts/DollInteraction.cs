using UnityEngine;
using UnityEngine.UI;

public class DollInteraction : MonoBehaviour, Interactable
{
    public string[] lines;
    public AudioClip[] clips;

    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
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
                if(UIManager.instance.dialogueBox.index >= lines.Length -1)
                {
                    UIManager.instance.dialogueBox.NextLine();
                    audioSource.Stop();
                    return;
                }
                audioSource.clip = clips[UIManager.instance.dialogueBox.index];
                audioSource.Play();
                UIManager.instance.dialogueBox.NextLine();
            }
        }
        else
        {
            UIManager.instance.dialogueBox.SetLines(lines);
            audioSource.clip = clips[0];
            audioSource.Play();
        }
        
    }
}
