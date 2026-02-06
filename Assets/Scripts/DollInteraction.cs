using UnityEngine;
using UnityEngine.UI;

public class DollInteraction : MonoBehaviour, Interactable
{
    public string[] lines;
    public AudioClip[] clips;

    public string name;

    public AudioSource audioSource;

    public PlayerController player;

    public UIManager uIManager;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindFirstObjectByType<PlayerController>();
        uIManager = FindFirstObjectByType<UIManager>();
    }
    public void Interact(PlayerController player)
    {
        
        if (uIManager.dialogueBox.gameObject.activeInHierarchy)
        {
            uIManager.dialogueBox.gameObject.SetActive(true);
            if (uIManager.dialogueBox.isScrolling)
            {
                uIManager.dialogueBox.CompleteLine();
            }
            else if (uIManager.dialogueBox.index >= lines.Length - 1)
            {
                uIManager.dialogueBox.NextLine();
                audioSource.Stop();
                player.bIsInteracting = false;
            }
            
            else
            {
                if(uIManager.dialogueBox.index >= lines.Length -1)
                {
                    uIManager.dialogueBox.NextLine();
                    audioSource.Stop();
                    player.bIsInteracting = false;
                    return;
                }
                audioSource.clip = clips[uIManager.dialogueBox.index];
                audioSource.Play();
                uIManager.dialogueBox.NextLine();
                
            }
        }
        else
        {
            player.bIsInteracting = true;
            uIManager.dialogueBox.SetLines(lines);
            uIManager.nameText.text = name;
            audioSource.clip = clips[0];
            audioSource.Play();
        }
        
    }
}
