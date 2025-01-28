using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FState
{

    float patrolTimer=30f;
    int k=0;
    public void OnEnter(FrybroCore f)
    {
         k = Random.Range(0, f.RoomPoints.Length-1);
        f.agent.SetDestination(f.RoomPoints[k].position);
        f.animator.SetFloat("speed", 1);
        AudioManager.instance.Play("snarl");
        Debug.Log("Im going here" + k + " father!");
    }

    public void OnExit(FrybroCore f)
    {
        //animation?
        f.animator.SetFloat("speed", 0);
        AudioManager.instance.Stop("snarl");
    }

    public void OnHurt(FrybroCore f)
    {
        f.ChangeState(f.chaseState);
    }

    public void UpdateState(FrybroCore f)
    {
        if (!(Vector3.Distance(f.RoomPoints[k].position, f.transform.position) < .3f))
        {
            f.agent.SetDestination(f.RoomPoints[k].position);
        }
        else
        {
            k = Random.Range(0, f.RoomPoints.Length - 1);
            f.agent.SetDestination(f.RoomPoints[k].position);
        }
        if (f.sight.canSee)
        {
            f.ChangeState(f.chaseState);
        }
    }
}
