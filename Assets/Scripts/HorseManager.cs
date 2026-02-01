using UnityEngine;

public class HorseManager : MonoBehaviour
{
    public AudioClip[] horseClips;

    public AudioSource audioSource;

    public PlayerController player;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindFirstObjectByType<PlayerController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
