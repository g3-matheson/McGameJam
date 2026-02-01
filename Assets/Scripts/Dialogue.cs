using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour
{
    private TextMeshProUGUI dialogueText;

    public GameObject characterPortrait;
    public float typingSpeed = 0.05f;
    public List<string> lines;
    public bool isScrolling = false;
    public int index;

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

    public void SetLines(string[] newLines)
    {
        lines.Clear();
        foreach (string line in newLines)
        {
            lines.Add(line);
        }
        gameObject.SetActive(true);
        StartDialogue();
    }

    private void StartDialogue()
    {
        index = 0;
        dialogueText.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isScrolling = true;
        // Type each character 1 by 1
            foreach (char c in lines[index].ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
        isScrolling = false;
    }

    public void CompleteLine()
    {
        StopAllCoroutines();
        isScrolling = false;
        dialogueText.text = lines[index];
    }

    public void NextLine()
    {
        if (index < lines.Count - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            index = 0;
            isScrolling = false;
            gameObject.SetActive(false);
        }
    }
}
