using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public Dialogue dialogueBox;
    public UILockScript lockPad;

    public TextMeshProUGUI nameText;

    void Awake()
    {
        dialogueBox = FindFirstObjectByType<Dialogue>();
        lockPad = FindFirstObjectByType<UILockScript>();
        nameText = GameObject.Find("Character Name").GetComponent<TextMeshProUGUI>();
        lockPad.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
    }
}
