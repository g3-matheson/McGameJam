using UnityEngine;

public class ScaleFromMicrophone : MonoBehaviour
{
    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioDetector audioDetector;

    public float loudnessSensitivity = 100.0f;
    public float threshhold = 0.1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = audioDetector.GetLoudNessFromMicrophone() * loudnessSensitivity;
        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);

        if(loudness < threshhold)
        {
            loudness = 0.0f;
        }
    }
}
