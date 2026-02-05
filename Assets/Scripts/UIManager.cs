using UnityEngine;

public class UIManager : MonoBehaviour
{

    public Dialogue dialogueBox;
    public UILockScript lockPad;

    void Awake()
    {
        dialogueBox = FindFirstObjectByType<Dialogue>();
        lockPad = FindFirstObjectByType<UILockScript>();
        lockPad.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
    }
}
