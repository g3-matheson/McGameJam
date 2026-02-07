using TMPro;
using UnityEngine;

public class LockInteraction : MonoBehaviour, Interactable
{
    UIManager uIManager;
    private BoxCollider2D boxCollider;

    private AudioSource audioSource;

    public AudioClip unlockSound;

    bool soundPlayed = false;
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        uIManager = FindFirstObjectByType<UIManager>();
        audioSource = GetComponent<AudioSource>();
    }

	void FixedUpdate()
	{
		if (uIManager.lockPad.gameObject.activeInHierarchy && uIManager.lockPad.isUnlocked)
        {
            uIManager.lockPad.gameObject.SetActive(false);
            HunterAI.Instance.playerController.bIsInteracting = false;
            HunterAI.Instance.playerController.OnEnable();
            Debug.Log("Lock unlocked, disabling lock." + "Sound played: " + soundPlayed + "Audio source is playing: " + audioSource.isPlaying);
            Debug.Log("Playing unlock sound.");
            audioSource.PlayOneShot(unlockSound);
            soundPlayed = true; 
            
        }
        else if (soundPlayed&& !audioSource.isPlaying)
        {
            Debug.Log("Sound already played, disabling lock.");
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
