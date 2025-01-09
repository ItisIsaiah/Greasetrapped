using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Minigame { 

    void SetUp();
    void GameLoop();
    void Abort();
    
}
