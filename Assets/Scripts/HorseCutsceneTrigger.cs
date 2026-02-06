using UnityEngine;

public class HorseCutsceneTrigger : MonoBehaviour
{

    HorseManager horseManager;
    PlayerController pc;

    [SerializeField] private int cutsceneStartIndex = 1;
    [SerializeField] private bool ignoreIfAmulet = false;

     void Awake()
    {
        horseManager = FindFirstObjectByType<HorseManager>();
        pc = FindFirstObjectByType<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player") || (ignoreIfAmulet && pc.bHasAmulet)) return;
        horseManager.PlayHorseSound(cutsceneStartIndex, true);
        gameObject.SetActive(false);
    }
}
