using UnityEngine;

public class DollInteraction : MonoBehaviour, Interactable
{
    public string[] lines;

    public AudioClip[] dollAudio;

    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
                UIManager.instance.dialogueBox.NextLine();
                if(dollAudio.Length <=0 ) return;
                audioSource.PlayOneShot(dollAudio[UIManager.instance.dialogueBox.index]);
            }
        }
        else
        {
            UIManager.instance.dialogueBox.SetLines(lines);
            if(dollAudio.Length > 0){ audioSource.PlayOneShot(dollAudio[0]); }
            player.bIsInteracting = true;
        }
        
    }
}
