using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] soundTracks;

    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.clip = soundTracks[0];
        audioSource.Play();

        StartCoroutine(PlayIntroMusic());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    System.Collections.IEnumerator PlayIntroMusic()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = soundTracks[1];
        audioSource.Play();
        audioSource.loop = true;
    }
}
