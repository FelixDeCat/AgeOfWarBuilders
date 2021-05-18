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
    protected override void OnInitialize()
    {
        UI_StateTimer.instance.Open();
        UI_StateTimer.Refresh(stateName);
    }
    protected override void OnEnd()
    {
        UI_StateTimer.instance.Close();
    }
    protected override void OnForceFinish() { }
    protected override void OnTick(float DeltaTime) { }
    
}
