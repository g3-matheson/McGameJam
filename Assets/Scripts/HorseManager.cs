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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayHorseSound(0);
        currentLines = new List<string>(lines).GetRange(0, 1).ToArray();
        PlayDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //Have horse follow the player smoothly
        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.fixedDeltaTime);
    }

    public void PlayHorseSound(int clipIndex,bool findLines=false)
    {
        player.bIsTalkingToHorse = true;
        audioSource.Stop();
        audioSource.clip = currentHorseClips[clipIndex];
        audioSource.Play();
        currentSoundIndex = clipIndex;
        if(findLines){
            for(int i=clipIndex+1; i<currentHorseClips.Length; i++){
                if(maxIndices.Contains(i)){
                    currentLines = new List<string>(lines).GetRange(clipIndex,i-clipIndex).ToArray();
                }
            }
        }
        uIManager.dialogueBox.SetLines(currentLines);
        player.bIsInteracting = true;
        uIManager.nameText.text = name;
        player.OnDisable();
        
    }

    public void PlayNextSound(){
        Debug.Log("Playing next sound");
        currentSoundIndex++;
        if(maxIndices.Contains(currentSoundIndex)){
            audioSource.Stop();
            player.bIsTalkingToHorse = false;
            return;
        }
        PlayHorseSound(currentSoundIndex);
    }

    public void PlayDialogue(){
         Debug.Log("Playing dialogue");
            if (uIManager.dialogueBox.isScrolling)
            {
                uIManager.dialogueBox.CompleteLine();
            }
            else if (uIManager.dialogueBox.index < currentLines.Length)
            {
                uIManager.dialogueBox.NextLine();
                PlayNextSound();
            }
            else
            {
                player.bIsInteracting = false;
                player.OnEnable();
                PlayNextSound();
            }
            
            // else
            // {
            //     if(uIManager.dialogueBox.index >= currentLines.Length -1)
            //     {
            //         player.bIsInteracting = false;
            //         player.OnEnable();
            //     }
            //     uIManager.dialogueBox.NextLine();
            //     PlayNextSound(); 
            // }
    }
}
