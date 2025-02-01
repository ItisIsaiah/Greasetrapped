using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurkState : FState
{
    float LurkTime = 15;
    float currtime = 0;

    float comebackTime = 15;
    float currComebackTime = 0;
    public void OnEnter(FrybroCore f)
    {
        currComebackTime = 0;
         currtime = 0;
        AudioManager.instance.Play("search");
        foreach(GameObject j in f.fries)
        {
            Fries js= j.GetComponent<Fries>();
            js.enabled = true;
            int k = Random.Range(0, f.fries.Length-1);
            js.FindCertainPlayer(f.playerList[k].transform,f.transform,f);
        }
    }

    public void OnExit(FrybroCore f)
    {
        foreach (GameObject j in f.fries)
        {
            Fries js = j.GetComponent<Fries>();

            js.transform.position = Vector3.zero;   
            js.ReturntoFrybro(f,false);
            
            js.enabled = false;
        }
        currComebackTime = 0;
        currtime = 0;

        
    }
    IEnumerator waitTimer()
    {
        yield return new WaitForSeconds(5);
    }
    public void OnHurt(FrybroCore f)
    {
        f.ChangeState(f.chaseState);
        //Return all the fries
    }

    public void UpdateState(FrybroCore f)
    {
        // throw new System.NotImplementedException();
        if (currtime <=LurkTime)
        {
            currtime += Time.deltaTime;
        }
        else
        {
            f.ChangeState(f.patrolState);
        }
        
    }

    
}
