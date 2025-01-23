using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FState
{
    public int sightTimer=3;
    public float cantsee=0;

    public void OnEnter(FrybroCore f)
    {
        cantsee = 0;
    }

    public void OnExit(FrybroCore f)
    {
       // throw new System.NotImplementedException();
    }

    public void OnHurt(FrybroCore f)
    {
        cantsee = -sightTimer;
    }

    public void UpdateState(FrybroCore f)
    {
        if (f.sight.canSee||cantsee>=sightTimer)
        {
            f.agent.SetDestination(f.sight.objectseen.transform.position);
            cantsee = 0;
        }
        else
        {
            cantsee += Time.deltaTime;
            f.ChangeState(f.patrolState);
        }
    }

   
    
}
