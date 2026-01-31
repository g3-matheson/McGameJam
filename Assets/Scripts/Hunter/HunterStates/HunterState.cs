using UnityEngine;

public abstract class HunterState
{
    public abstract void Enter(HunterAI ai);
    public abstract void Exit(HunterAI ai);
    public abstract void Tick(HunterAI ai);
}