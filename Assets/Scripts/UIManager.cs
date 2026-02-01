using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Dialogue dialogueBox;
    public GameObject paintingImage;
    public UILockScript lockPad;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


}
