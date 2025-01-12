using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MashingGame : MonoBehaviour, Minigame
{
    bool taskCompleted = false;
    int timesHit;
    int timesNeededToHit;
    public void GameLoop(MinigameManager m)
    {
        if (timesHit == timesNeededToHit)
        {
            WinGame(m);
        }
    }

    public void SetUp()
    {
        taskCompleted = false;
        timesHit = 0;
        timesNeededToHit = Random.Range(25,50);
    }


    public void Abort()
    {
        taskCompleted = false;
        timesHit = 0;
    }


    public void WinGame(MinigameManager m)
    {
        m.CompletedTask();
    }

    public void addOne()
    {
        timesHit++;
    }


    private void OnDisable()
    {
        Debug.Log("I was disabled");
    }

}
