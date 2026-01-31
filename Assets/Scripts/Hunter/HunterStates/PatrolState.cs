using UnityEngine;

public class PatrolState : HunterState
{
    public int CurrentPatrolIndex;
    private float ArriveThreshold = 0.5f;
    public override void Enter()
    {
        FindPatrolIndex();
    }

    private void FindPatrolIndex()
    {
        int index = 0;
        float shortest = float.MaxValue;

        for (int i = 0; i < HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom].Count; i++)
        {
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

    public override void Exit()
    {
    }

    public override void Tick()
    {
        // TODO If Hunter is not in the Hallway, should return to the Hallway (ALONG NAVMESH) before Patrolling Hallway
        // call FindPatrolIndex again afterwards
        Vector2 dir = HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom][CurrentPatrolIndex].position - HunterAI.Instance.Hunter.transform.position;
        HunterAI.Instance.HunterRB.linearVelocity = dir.normalized * HunterAI.Instance.HunterSpeed;

        if (dir.magnitude < ArriveThreshold)
            CurrentPatrolIndex = (CurrentPatrolIndex + 1) % HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom].Count;
    }
}