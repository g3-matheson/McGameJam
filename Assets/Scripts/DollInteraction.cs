using UnityEngine;
using UnityEngine.UI;

public class DollInteraction : MonoBehaviour, Interactable
{
    public string[] lines;
    public AudioClip[] clips;

    public AudioSource audioSource;

    public PlayerController player;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindFirstObjectByType<PlayerController>();
    }
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
                audioSource.Stop();
                player.bIsInteracting = false;
            }
            
            else
            {
                if(UIManager.instance.dialogueBox.index >= lines.Length -1)
                {
                    UIManager.instance.dialogueBox.NextLine();
                    audioSource.Stop();
                    player.MoveAction.Enable();
                    return;
                }
                audioSource.clip = clips[UIManager.instance.dialogueBox.index];
                audioSource.Play();
                UIManager.instance.dialogueBox.NextLine();
                //if(dollAudio.Length <=0 ) return;
                //audioSource.PlayOneShot(dollAudio[UIManager.instance.dialogueBox.index]);
            }
        }
        else
        {
            player.MoveAction.Disable();
            UIManager.instance.dialogueBox.SetLines(lines);
            audioSource.clip = clips[0];
            audioSource.Play();
        }
        
    }
}
