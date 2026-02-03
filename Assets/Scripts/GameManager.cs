using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    public Room PlayerCurrentRoom = Room.GirlRoom;

    public bool bWin = false;

    public Dictionary<Room, int> MaxRats = new()
    {
        { Room.GirlRoom, 2 },
        { Room.DadOffice, 4},
        { Room.DiningRoom, 5},
        { Room.SisterRoom, 2},
        { Room.Hallway, 6}
    };

    public Dictionary<Room, int> CurrentRats = new()
    {
        { Room.GirlRoom, 0 },
        { Room.DadOffice, 0},
        { Room.DiningRoom, 0},
        { Room.SisterRoom, 0},
        { Room.Hallway, 0 }
    };

    public List<RatSpawner> RatSpawners;

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

    public void ReloadScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        var objs = GameObject.FindGameObjectsWithTag("RatSpawner");
        foreach (var o in objs)
        {
            RatSpawners.Add(o.GetComponent<RatSpawner>());
        }
        
        for (int i = 0; i < 5; i++)
        {
            CurrentRats[(Room)i] = 0;
        }

    } 
}