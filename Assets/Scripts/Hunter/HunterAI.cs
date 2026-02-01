using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
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

    public Dictionary<int, GameManager.Room> DoorChecks = new()
    {
        { 0, GameManager.Room.GirlRoom },
        { 2, GameManager.Room.DiningRoom },
        { 4, GameManager.Room.SisterRoom }
    };

    public GameManager.Room CurrentRoom;
    public HunterState CurrentState;
    public GameObject Hunter;

    // 2D Navmesh Package: https://github.com/h8man/NavMeshPlus
    public NavMeshAgent HunterAgent;

    public float RaycastDistance = 10f;
    public LayerMask RaycastObscureLayer;
    public bool ChasingPlayer = false;
    private Footsteps footsteps;
    public GameObject Player;
    public PlayerController playerController;
    public float HearingRange = 10f;

    public float ArriveThreshold = 3f;

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
        HunterAgent = Hunter.GetComponent<NavMeshAgent>();
		HunterAgent.updateRotation = false;
		HunterAgent.updateUpAxis = false;

        CurrentState = new PatrolState();
        CurrentRoom = GameManager.Room.Hallway;

        Player = GameObject.Find("Player");
        playerController = Player.GetComponent<PlayerController>();
        footsteps = GetComponent<Footsteps>();
        
        GameObject hallwayPatrolPoints = GameObject.Find("HallwayPatrolPoints");
        AddPatrolPoints(GameManager.Room.Hallway, hallwayPatrolPoints);

        GameObject girlRoomPatrolPoints = GameObject.Find("GirlRoomPatrolPoints");
        AddPatrolPoints(GameManager.Room.GirlRoom, girlRoomPatrolPoints);

        //GameObject diningRoomPatrolPoints = GameObject.Find("DiningRoomPatrolPoints");
        //AddPatrolPoints(GameManager.Room.DiningRoom, diningRoomPatrolPoints);

        //GameObject sisterRoomPatrolPoints = GameObject.Find("SisterRoomPatrolPoints");
        //AddPatrolPoints(GameManager.Room.SisterRoom, sisterRoomPatrolPoints);

        CurrentState.Enter();
    }

    private void AddPatrolPoints(GameManager.Room room, GameObject pts) => PatrolPoints.Add(room, pts.GetComponentsInChildren<Transform>().Where(t => !t.Equals(pts.transform)).ToList());

    void FixedUpdate()
    {
        if (!ChasingPlayer && TryRaycastPlayer())
        {
            KillPlayer();
        }

        if (playerController.bIsHiding)
        {
            ChasingPlayer = false;
            CurrentRoom = GameManager.Room.Hallway;
            CurrentState = new PatrolState();
            CurrentState.Enter();
            HunterAgent.speed = 2f;
            footsteps.Tick = 0.75f;
        }

        CurrentState?.Tick();

        footsteps.Active = HunterAgent.velocity.magnitude > 0.25f;
    }

    public void PlayerShout(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (CurrentRoom != GameManager.Room.Hallway && CurrentRoom != GameManager.Instance.PlayerCurrentRoom)
        {
            // go to player room and patrol
        }
        else if (CurrentRoom != GameManager.Room.Hallway) // player in same room
        {
            KillPlayer();
        }
        else if (GameManager.Instance.PlayerCurrentRoom == GameManager.Room.Hallway && GameManager.Instance.PlayerCurrentRoom == GameManager.Room.Hallway) // both are in the hallway
        {
            Vector3 dir = Player.transform.position - Hunter.transform.position;
            float distance = new Vector2(dir.x, dir.y).magnitude;
            if (distance < HearingRange) KillPlayer();
            else MoveToPlayer(GameManager.Room.Hallway);
        }
        else
        {
            MoveToPlayer(GameManager.Instance.PlayerCurrentRoom); // hunter in hallway, player in a room
        }
    }

    void KillPlayer()
    {
        CurrentState = new SeekState(GameManager.Instance.PlayerCurrentRoom, Player.transform.position, true);
        HunterAgent.speed *= 3;
        footsteps.Tick /= 3; 
        ChasingPlayer = true;
    }

    void MoveToPlayer(GameManager.Room room)
    {
        CurrentState = new SeekState(room, Player.transform.position);
    }

    public void SwitchToPatrol(GameManager.Room room)
    {
        CurrentRoom = room;
        CurrentState = new PatrolState();
        CurrentState.Enter();
    }

    void LateUpdate()
    {
        var pos = Hunter.transform.position;
        pos.z = 0f;
        Hunter.transform.position = pos;
    }

    bool TryRaycastPlayer()
    {
        Vector3 dir = Player.transform.position - Hunter.transform.position;
        Vector2 direction = new Vector2(dir.x, dir.y).normalized;
        Vector2 startPos = new Vector2(Hunter.transform.position.x, Hunter.transform.position.y);
    
        RaycastHit2D ray = Physics2D.Raycast(startPos, direction, RaycastDistance, RaycastObscureLayer);

        if (ray && ray.transform.CompareTag("Player")) return true;
        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Hunter.transform.position, RaycastDistance); 
    }
#endif
}