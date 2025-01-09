using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FState
{
    public void OnEnter(FrybroCore f)
    {
        int k = Random.Range(0, f.RoomPoints.Length-1);
        f.agent.SetDestination(f.RoomPoints[k].position);
    }

    public void OnExit(FrybroCore f)
    {
        throw new System.NotImplementedException();
    }

    public void OnHurt(FrybroCore f)
    {
        f.ChangeState(f.chaseState);
    }

    public void UpdateState(FrybroCore f)
    {
        throw new System.NotImplementedException();
    }
}
