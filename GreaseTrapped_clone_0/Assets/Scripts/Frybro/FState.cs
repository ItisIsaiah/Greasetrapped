using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FState 
{
    void UpdateState(FrybroCore f);
    void OnEnter(FrybroCore f);
    void OnExit(FrybroCore f);
    void OnHurt(FrybroCore f);
}
