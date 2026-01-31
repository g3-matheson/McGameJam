using UnityEngine;

public class HunterAI : MonoBehaviour
{
    public static HunterAI Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }    

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}