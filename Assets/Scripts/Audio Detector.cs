using UnityEngine;

public class AudioDetector : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    private string device = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MicrophoneToAudioClip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MicrophoneToAudioClip()
    {
        //Get the first microphone device
        for(int i=0; i<Microphone.devices.Length; i++)
        {
            Debug.Log("Microphone device " + i + ": " + Microphone.devices[i]);
            if(Microphone.devices[i] == "DualSense wireless controller (PS5) Analog Stereo")
            {
                device = Microphone.devices[i];
                Debug.Log("Found preferred microphone device: " + device);
                break;
            }
        }
        Debug.Log("Using microphone: " + device);

        microphoneClip = Microphone.Start(device, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudNessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(null), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow + 1;

        if(startPosition < 0)
        {
            return 0.0f;
        }

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        //compute Loudness
        float totalLoudness = 0.0f;
        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}
