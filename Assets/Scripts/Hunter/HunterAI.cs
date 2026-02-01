using UnityEngine;
using UnityEngine.AI;
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
        {GameManager.Room.GirlRoom, 0},   
        {GameManager.Room.DiningRoom, 2},   
        {GameManager.Room.SisterRoom, 4},     
        {GameManager.Room.DadOffice, 0},    
    };

    public GameManager.Room CurrentRoom;
    public HunterState CurrentState;
    public GameObject Hunter;
    public Rigidbody2D HunterRB;

    // 2D Navmesh Package: https://github.com/h8man/NavMeshPlus
    public NavMeshAgent HunterAgent;

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
        HunterAgent = Hunter.GetComponent<NavMeshAgent>();
		HunterAgent.updateRotation = false;
		HunterAgent.updateUpAxis = false;

        CurrentState = new PatrolState();
        CurrentRoom = GameManager.Room.Hallway;
        
        GameObject hallwayPatrolPoints = GameObject.Find("HallwayPatrolPoints");
        if (hallwayPatrolPoints == null) Debug.Log($"Hallway pts not found");
        AddPatrolPoints(GameManager.Room.Hallway, hallwayPatrolPoints);

        CurrentState.Enter();
    }

    private void AddPatrolPoints(GameManager.Room room, GameObject pts) => PatrolPoints.Add(room, pts.GetComponentsInChildren<Transform>().Where(t => !t.Equals(pts.transform)).ToList());

    void FixedUpdate()
    {
        CurrentState.Tick();
    }
}