using UnityEngine;
using System.Collections.Generic;


public class SeekState : HunterState
{
    public GameManager.Room SeekRoom;
    public Vector2 SeekLocation;
    private List<Transform> Points;
    private Transform TargetPoint;

    public SeekState(GameManager.Room room, Vector2 location)
    {
        SeekRoom = room;
        SeekLocation = location;       
        if (SeekRoom != GameManager.Room.Hallway)
        {
            TargetPoint = HunterAI.Instance.PatrolPoints[GameManager.Room.Hallway][HunterAI.Instance.RoomEntrances[SeekRoom]];
        }
        // TODO else if Player is in range, go directly to them
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

    public override void Enter()
    {

    }

    public override void Exit()
    {
        
    }

    public override void Tick()
    {
        // move towards TargetPoint along NavMesh
    }
}