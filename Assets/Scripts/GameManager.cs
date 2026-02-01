using UnityEngine;
using UnityEngine.SceneManagement;

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
    } 
}