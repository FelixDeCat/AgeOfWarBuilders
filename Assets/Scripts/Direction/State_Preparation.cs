using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Preparation : StateElement
{
    public float time_to_end;
    float timer;
    bool start_countdown;

    protected override void OnBegin()
    {
        start_countdown = true;
        timer = time_to_end;
        UI_StateTimer.instance.Open();
    }

    protected override void OnEnd()
    {
        start_countdown = false;
        timer = time_to_end;
        UI_StateTimer.instance.Close();
    }

    protected override void OnTick(float DeltaTime)
    {
        if (timer > 0)
        {
            timer = timer - 1 * DeltaTime;
            UI_StateTimer.Refresh(stateName, timer);
        }
        else
        {
            timer = time_to_end;
            ForceFinish();
        }
    }

    protected override void OnForceFinish()
    {
        
    }
}
