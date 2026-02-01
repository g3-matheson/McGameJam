using UnityEngine;

public abstract class HunterState
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Tick();
}