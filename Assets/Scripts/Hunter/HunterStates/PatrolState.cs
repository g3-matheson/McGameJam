using UnityEngine;

public class PatrolState : HunterState
{
    public int CurrentPatrolIndex;

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

    public override void Exit() { }

    public override void Tick()
    {
        HunterAI.Instance.HunterAgent.SetDestination(HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom][CurrentPatrolIndex].position);
        if ((HunterAI.Instance.Hunter.transform.position - HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom][CurrentPatrolIndex].position).magnitude < HunterAI.Instance.ArriveThreshold)
        {
            CurrentPatrolIndex = (CurrentPatrolIndex + 1) % HunterAI.Instance.PatrolPoints[HunterAI.Instance.CurrentRoom].Count;
            if (HunterAI.Instance.CurrentRoom != GameManager.Room.Hallway && CurrentPatrolIndex == 0)
                HunterAI.Instance.SwitchToPatrol(GameManager.Room.Hallway);
        }
            
    }
}