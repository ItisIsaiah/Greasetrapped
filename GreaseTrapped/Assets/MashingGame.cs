using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashingGame : Minigame
{
    bool taskCompleted = false;
    int timesHit;
    int timesNeededToHit;
    public void GameLoop()
    {
        if (timesHit == timesNeededToHit)
        {

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

    public void addOne()
    {
        timesHit++;
    }


}
