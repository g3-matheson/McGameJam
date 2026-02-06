using UnityEngine;
using System.Collections.Generic;

public class HorseManager : MonoBehaviour
{
    public AudioClip[] currentHorseClips;

    public string name = "Horse";

    public string[] lines;

    public string[] currentLines;

    public AudioSource audioSource;

    public PlayerController player;

    public UIManager uIManager;

    public int currentSoundIndex = 0;
    List<int> maxIndices = new List<int>(){1,5,6,7};

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindFirstObjectByType<PlayerController>();
        uIManager = FindFirstObjectByType<UIManager>();
    }
    void Start()
    {
        currentLines = new List<string>(lines).GetRange(0, 1).ToArray();
        PlayHorseSound(0);
        HorseDialogueInteract();
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.fixedDeltaTime);
    }

    void StartHorseInteraction()
    {
        player.bIsInteracting = true;
        player.bIsTalkingToHorse = true;
        player.OnDisable();
    }

    void EndHorseInteraction()
    {
        player.bIsInteracting = false;
        player.bIsTalkingToHorse = false;
        player.OnEnable();
        uIManager.dialogueBox.gameObject.SetActive(false);
    }

    public void PlayHorseSound(int clipIndex, bool findLines=false)
    {
        StartHorseInteraction();
        Debug.Log($"Playing sound {clipIndex}");
        
        audioSource.Stop();
        audioSource.clip = currentHorseClips[clipIndex];
        audioSource.Play();
        currentSoundIndex = clipIndex;
        if(findLines){
            for(int i=clipIndex+1; i<=currentHorseClips.Length; i++){
                if(maxIndices.Contains(i)){
                    currentLines = new List<string>(lines).GetRange(clipIndex,i-clipIndex).ToArray();
                }
            }
        }
        foreach (var c in currentLines) Debug.Log(c);
        uIManager.dialogueBox.SetLines(currentLines);
        uIManager.nameText.text = name;
    }

    public void PlayNextSound(){
        currentSoundIndex++;
        if(maxIndices.Contains(currentSoundIndex)){
            audioSource.Stop();
            EndHorseInteraction(); 
            return;
        }
        PlayHorseSound(currentSoundIndex, true);
    }

    public void HorseDialogueInteract(){
        if (uIManager.dialogueBox.isScrolling)
        {
            uIManager.dialogueBox.CompleteLine();
            return;
        }
        else if (uIManager.dialogueBox.index < currentLines.Length - 1)
        {
            uIManager.dialogueBox.NextLine();
            PlayNextSound();
        }
        else // no lines left
        {
            PlayNextSound();
        }
    }
}