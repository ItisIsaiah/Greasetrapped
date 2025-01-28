using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FState
{
    public int sightTimer=3;
    public float cantsee=0;

    public void OnEnter(FrybroCore f)
    {
        f.animator.SetFloat("speed",1);
        f.animator.SetBool("aggressive",true);
        AudioManager.instance.Play("scream");
        cantsee = 0;
    }

    public void OnExit(FrybroCore f)
    {
        f.animator.SetBool("aggressive", false);
        f.animator.SetFloat("speed", 0);
        AudioManager.instance.Stop("scream");
       
        // throw new System.NotImplementedException();
    }

    public void OnHurt(FrybroCore f)
    {
        cantsee = -sightTimer;
    }

    public void UpdateState(FrybroCore f)
    {
        if (f.sight.canSee||cantsee<=sightTimer)
        {
            if (!f.sight.canSee)
            {
                cantsee += Time.deltaTime;
            }
            else
            {
                cantsee = 0;
            }


            f.agent.SetDestination(f.sight.objectseen.transform.position);
            cantsee = 0;

            if (Vector3.Distance(f.transform.position, f.sight.objectseen.transform.position) < .8f)
            {
                f.sight.objectseen.GetComponent<PlayerController>().Die();
            }
        }
        else
        {
            //cantsee += Time.deltaTime;
            f.ChangeState(f.patrolState);
        }


       
    }

   
    
}
