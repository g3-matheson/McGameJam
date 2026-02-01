using UnityEngine;
using System.Collections;
using TMPro;

public class Dialogue : MonoBehaviour
{
    private TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    public string[] lines;

    private int index;

    void Awake()
    {
        //find the child of this object and get its TextMeshProUGUI component
        dialogueText = GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.text = string.Empty;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // Type each character 1 by 1
            foreach (char c in lines[index].ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            index = 0;
            gameObject.SetActive(false);
        }
    }
}
