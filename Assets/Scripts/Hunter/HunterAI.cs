using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class HunterAI : MonoBehaviour
{
    public static HunterAI Instance { get; private set; }

    // Room number -> List of Patrol points
    public Dictionary<GameManager.Room, List<Transform>> PatrolPoints = new();
    
    // Room number -> Hallway Patrol point to go to 
    public Dictionary<GameManager.Room, int> RoomEntrances = new()
    {
        {GameManager.Room.GirlRoom, 1},   
        {GameManager.Room.DiningRoom, 5},   
        {GameManager.Room.SisterRoom, 6},     
        {GameManager.Room.DadOffice, 2},    
    };

    public GameManager.Room CurrentRoom;
    public HunterState CurrentState;
    public GameObject Hunter;
    public Rigidbody2D HunterRB;
    public float HunterSpeed = 5.0f;

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

    public void Start()
    {
        Hunter = GameObject.Find("Hunter");  
        HunterRB = Hunter.GetComponent<Rigidbody2D>(); 
        CurrentState = new PatrolState();
        CurrentRoom = GameManager.Room.Hallway;
        
        GameObject hallwayPatrolPoints = GameObject.Find("HallwayPatrolPoints");
        if (hallwayPatrolPoints == null) Debug.Log($"Hallway pts not found");
        AddPatrolPoints(GameManager.Room.Hallway, hallwayPatrolPoints);
    }

    private void AddPatrolPoints(GameManager.Room room, GameObject pts) => PatrolPoints.Add(room, pts.GetComponentsInChildren<Transform>().Where(t => !t.Equals(pts.transform)).ToList());

    void Update()
    {
        CurrentState.Tick();
    }
}