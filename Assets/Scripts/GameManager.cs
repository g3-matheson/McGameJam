using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Room
    {
        Hallway = 0,
        GirlRoom = 1,
        DiningRoom = 2,
        SisterRoom = 3,
        DadOffice = 4   
    }

    public static GameManager Instance { get; private set; }

    public Room HunterCurrentRoom => HunterAI.Instance.CurrentRoom;

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