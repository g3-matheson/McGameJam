using System.Runtime.Serialization;
using UnityEngine;

public class HorseCutsceneTrigger : MonoBehaviour
{
    BoxCollider2D cutsceneTrigger;

    HorseManager horseManager;

    [SerializeField] private int cutsceneStartIndex = 1;

     void Awake()
    {
        horseManager = FindFirstObjectByType<HorseManager>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cutsceneTrigger = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
