using UnityEngine;
using System.Collections.Generic;

public class SeekState : HunterState
{
    public GameManager.Room SeekRoom;
    public Vector2 SeekLocation;
    private List<Transform> Points;
    private Transform TargetPoint;
    private bool KillPlayer;

    public SeekState(GameManager.Room room, Vector2 location, bool killPlayer = false)
    {
        KillPlayer = killPlayer;
        SeekRoom = room;
        SeekLocation = location;       

        if (KillPlayer) return;

        if (SeekRoom != GameManager.Room.Hallway && SeekRoom != HunterAI.Instance.CurrentRoom)
        {
            TargetPoint = HunterAI.Instance.PatrolPoints[GameManager.Room.Hallway][HunterAI.Instance.RoomEntrances[SeekRoom]];
        }
        else
        {
            Points = HunterAI.Instance.PatrolPoints[GameManager.Room.Hallway];
            int index = 0;
            float minimumDist = float.MaxValue;
            for (int i = 0; i < Points.Count; i++)
            {
                float dist = (HunterAI.Instance.Hunter.transform.position - Points[i].transform.position).magnitude;
                if (dist < minimumDist)
                {
                    minimumDist = dist;
                    index = i;
                }
            }
            TargetPoint = Points[index];
        }
    }

    public override void Enter() { }

    public override void Exit() { }

    public override void Tick()
    {
        if (HunterAI.Instance.playerController.bIsTalkingToHorse) return;
        if (KillPlayer)
        {
            HunterAI.Instance.HunterAgent.SetDestination(HunterAI.Instance.Player.transform.position);
        }
        else
        {
            HunterAI.Instance.HunterAgent.SetDestination(TargetPoint.transform.position);
            if ((HunterAI.Instance.Hunter.transform.position - TargetPoint.transform.position).magnitude < HunterAI.Instance.ArriveThreshold)
            {
                HunterAI.Instance.SwitchToPatrol(SeekRoom);
            }
        }
    }
}