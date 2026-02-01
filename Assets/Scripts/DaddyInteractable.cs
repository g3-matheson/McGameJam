using UnityEngine;

public class DaddyInteractable : MonoBehaviour, Interactable
{
    
    public SpriteRenderer _spriteRenderer;
    public Sprite _sprite;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Interact(PlayerController player)
    {
        player.bHasAmulet = true;
        _spriteRenderer.sprite = _sprite;
    }
}
