using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public GameObject paintingImage;

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
        paintingImage.SetActive(false);
    }


}
