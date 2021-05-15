using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Finish : StateElement
{
    protected override void OnBegin()
    {
        GameLoop.Pause();
        GameLoop.Win();
    }
    protected override void OnEnd() { }
    protected override void OnForceFinish() { }
    protected override void OnTick(float DeltaTime) { }
}
