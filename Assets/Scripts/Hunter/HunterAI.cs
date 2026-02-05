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
    private Animator HunterAnimator;

    public float RaycastDistance = 10f;
    public LayerMask RaycastObscureLayer;
    public bool ChasingPlayer = false;
    private Footsteps footsteps;
    public GameObject Player;
    public PlayerController playerController;
    public float HearingRange = 10f;

    public float ArriveThreshold = 5f;

    public float FearTimer = 60f;
    public float RandomRoomCooldown = 20f;
    public float RandomRoomTimer = 30f;
    public bool CanGoInRandomRoom = false;

    public bool Paused = false;

    public float KillDistance = 5f;

    void Awake()
    {
        Instance = this;

        GameObject hallwayPatrolPoints = GameObject.Find("HallwayPatrolPoints");
        AddPatrolPoints(GameManager.Room.Hallway, hallwayPatrolPoints);

        GameObject girlRoomPatrolPoints = GameObject.Find("GirlRoomPatrolPoints");
        AddPatrolPoints(GameManager.Room.GirlRoom, girlRoomPatrolPoints);

        GameObject diningRoomPatrolPoints = GameObject.Find("DiningRoomPatrolPoints");
        AddPatrolPoints(GameManager.Room.DiningRoom, diningRoomPatrolPoints);

        GameObject sisterRoomPatrolPoints = GameObject.Find("SisterRoomPatrolPoints");
        AddPatrolPoints(GameManager.Room.SisterRoom, sisterRoomPatrolPoints);
    }

    public void Start()
    {
        Hunter = GameObject.Find("Hunter");  
        HunterAgent = Hunter.GetComponent<NavMeshAgent>();
		HunterAgent.updateRotation = false;
		HunterAgent.updateUpAxis = false;

        HunterAnimator = Hunter.GetComponent<Animator>();

        CurrentState = new PatrolState();
        CurrentRoom = GameManager.Room.Hallway;
        Player = GameObject.Find("Player");
        playerController = Player.GetComponent<PlayerController>();
        footsteps = Hunter.GetComponent<Footsteps>();

        CanGoInRandomRoom = false;
        RandomRoomTimer = 30f;
        CurrentState.Enter();
    }

    private void AddPatrolPoints(GameManager.Room room, GameObject pts) => PatrolPoints.Add(room, pts.GetComponentsInChildren<Transform>().Where(t => !t.Equals(pts.transform)).ToList());

    void Update()
    {
        if (Paused) return;
        if (RandomRoomTimer > 0) RandomRoomTimer = Mathf.Clamp(RandomRoomTimer - Time.deltaTime, 0, float.MaxValue);    
        CanGoInRandomRoom = RandomRoomTimer == 0f;

        float playerDist = (Hunter.transform.position - Player.transform.position).magnitude;
        if (playerDist < KillDistance && !playerController.bIsHiding)
        {
            playerController.Death();
            HunterAnimator.SetTrigger("Attack");
        }


        if (ChasingPlayer && playerController.bIsHiding)
        {
            ChasingPlayer = false;
            SwitchToPatrol(GameManager.Room.Hallway);
            HunterAgent.speed = 2f;
            footsteps.Tick = 0.75f;
        }

        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        if (playerController.bIsDead) return;
        
        var movementInput = HunterAgent.velocity.normalized;
        HunterAnimator.SetBool("IsMoving", movementInput.magnitude > 0);
        var threshold = 0.25f;
        var moveX = movementInput.x > threshold ? 1 : movementInput.x < -threshold ? -1 : 0;
        var moveY = movementInput.y > threshold ? 1 : movementInput.y < -threshold ? -1 : 0;
        HunterAnimator.SetFloat("MoveX", moveX);
        HunterAnimator.SetFloat("MoveY", moveY);

        var dir = (Player.transform.position - (Hunter.transform.position + new Vector3(0, 0.6f, 0))).normalized;
        var attackX = dir.x > threshold ? 1 : dir.x < -threshold ? -1 : 0;
        var attackY = dir.y > threshold ? 1 : dir.y < -threshold ? -1 : 0;
        HunterAnimator.SetFloat("AttackX", attackX);
        HunterAnimator.SetFloat("AttackY", attackY);
    }

    void FixedUpdate()
    {
        if (Paused) return;
        if (!ChasingPlayer && TryRaycastPlayer())
        {
            KillPlayer();
        }

        CurrentState?.Tick();
        footsteps.Active = HunterAgent.velocity.magnitude > 0.25f;
    }

    void LateUpdate()
    {
        if (Paused) return;
        var pos = Hunter.transform.position;
        pos.z = 0f;
        Hunter.transform.position = pos;

    }


    public void PlayerShout(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        playerController.PlayerAnimator.SetTrigger("Shout");

        if (CurrentRoom != GameManager.Room.Hallway && CurrentRoom != GameManager.Instance.PlayerCurrentRoom)
        {
            MoveToPlayer(GameManager.Instance.PlayerCurrentRoom);
        }
        else if (CurrentRoom != GameManager.Room.Hallway) // player in same room
        {
            KillPlayer();
        }
        else if (GameManager.Instance.PlayerCurrentRoom == GameManager.Room.Hallway && GameManager.Instance.PlayerCurrentRoom == GameManager.Room.Hallway) // both are in the hallway
        {
            Vector3 dir = Player.transform.position - Hunter.transform.position;
            float distance = new Vector2(dir.x, dir.y).magnitude;
            if (distance < HearingRange && TryRaycastPlayer()) KillPlayer();
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

    public void MoveToPlayer(GameManager.Room room)
    {
        CurrentState = new SeekState(room, Player.transform.position);
    }

    public void ResetRandomRoomTimer()
    {
        RandomRoomTimer = RandomRoomCooldown;    
    }

    public void SwitchToPatrol(GameManager.Room room)
    {
        CurrentRoom = room;
        CurrentState = new PatrolState();
        CurrentState.Enter();
    }
    bool TryRaycastPlayer()
    {
        if (playerController.bIsHiding) return false;

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