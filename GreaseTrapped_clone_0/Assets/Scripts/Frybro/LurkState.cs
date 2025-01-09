using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurkState : FState
{
    public void OnEnter(FrybroCore f)
    {
        foreach(GameObject j in f.fries)
        {
            Fries js= j.GetComponent<Fries>();
            int k = Random.Range(0, f.fries.Length-1);
            js.FindCertainPlayer(f.playerList[k].transform,f.transform,this);
        }
    }

    public void OnExit(FrybroCore f)
    {
        throw new System.NotImplementedException();
        //Return all the fries
    }

    public void OnHurt(FrybroCore f)
    {
        throw new System.NotImplementedException();
        //Return all the fries
    }

    public void UpdateState(FrybroCore f)
    {
        throw new System.NotImplementedException();
    }
}
