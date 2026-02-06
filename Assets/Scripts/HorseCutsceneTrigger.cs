using UnityEngine;

public class HorseCutsceneTrigger : MonoBehaviour
{

    HorseManager horseManager;

    [SerializeField] private int cutsceneStartIndex = 1;

     void Awake()
    {
        horseManager = FindFirstObjectByType<HorseManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player")) return;
        horseManager.PlayHorseSound(cutsceneStartIndex,true);
        horseManager.PlayDialogue();
        gameObject.SetActive(false);
    }
}
