using UnityEngine;

public class DaddyInteractable : MonoBehaviour, Interactable
{
    
    public SpriteRenderer _spriteRenderer;
    public Sprite _sprite;

    private AudioSource audioSource;

    public AudioClip pickupSound;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    public void Interact(PlayerController player)
    {
        player.bHasAmulet = true;
        _spriteRenderer.sprite = _sprite;
        audioSource.PlayOneShot(pickupSound);
    }
}
