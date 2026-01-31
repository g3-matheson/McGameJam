using UnityEngine;

public class ScaleFromAudioClip : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    public AudioDetector audioDetector;

    public float loudnessSensitivity = 100.0f;
    public float threshhold = 0.1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioDetector = FindFirstObjectByType<AudioDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = audioDetector.GetLoudnessFromAudioClip(source.timeSamples, source.clip) * loudnessSensitivity;
        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);

        if(loudness < threshhold)
        {
            loudness = 0.0f;
        }
    }
}
