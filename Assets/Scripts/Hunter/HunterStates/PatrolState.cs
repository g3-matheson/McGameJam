using UnityEngine;
using System.Linq;

public class PatrolState : HunterState
{
    public int CurrentPatrolIndex;
    private float Timer = 0f;

    public override void Enter()
    {
       FindPatrolIndex();
       Timer = 0f;
    }

    private void FindPatrolIndex()
    {
        int index = 0;
        float shortest = float.MaxValue;

        for (int i = 0; i < HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom].Count; i++)
        {
            Debug.Log($"Hunter? {HunterAI.Instance.Hunter == null}");
            Debug.Log($"PatrolPoints? {HunterAI.Instance.PatrolPoints == null}");
            Debug.Log($"CurrentRoom? {HunterAI.Instance.CurrentRoom}");
            float dist = (HunterAI.Instance.Hunter.transform.position
                        - HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom][i].position).magnitude;
            if (dist < shortest)
            {
                shortest = dist;
                index = i;
            }
        }

        CurrentPatrolIndex = index;
    }

    public override void Exit() { }

    public override void Tick()
    {
        if (HunterAI.Instance.CurrentRoom == GameManager.Room.Hallway) 
        {
            Timer += Time.deltaTime;
            if (Timer > HunterAI.Instance.FearTimer)
                HunterAI.Instance.MoveToPlayer(GameManager.Instance.PlayerCurrentRoom);
        }

        HunterAI.Instance.HunterAgent.SetDestination(HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom][CurrentPatrolIndex].position);
        if ((HunterAI.Instance.Hunter.transform.position - HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom][CurrentPatrolIndex].position).magnitude < HunterAI.Instance.ArriveThreshold)
        {
            if (HunterAI.Instance.CanGoInRandomRoom
             && HunterAI.Instance.CurrentRoom == GameManager.Room.Hallway 
             && HunterAI.Instance.DoorChecks.TryGetValue(CurrentPatrolIndex, out var Room)
             && Random.value > 0.5f)
            {
                HunterAI.Instance.ResetRandomRoomTimer();
                HunterAI.Instance.SwitchToPatrol(Room);
            }

            CurrentPatrolIndex = (CurrentPatrolIndex + 1) % HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom].Count;
            if (HunterAI.Instance.CurrentRoom != GameManager.Room.Hallway && CurrentPatrolIndex == 0)
            {
                HunterAI.Instance.ResetRandomRoomTimer();
                HunterAI.Instance.SwitchToPatrol(GameManager.Room.Hallway);
            }
                
        }
            
    }
}