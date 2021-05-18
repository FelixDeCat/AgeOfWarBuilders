using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public class BurstExecuter
{
    //por si se lo queremos pasar por callback
    public Action Execute;
    public Action OnFinish;

    bool exe;

    float timer;
    [SerializeField] float time_space_between_executions = 0.2f;
    [SerializeField] int burstCant = 3;
    public int BurstCant => burstCant;
    int currentBurstCant;

    public void Configure_Basics(int burstCant = 3, float time_between_executions = 0.2f)
    {
        this.time_space_between_executions = time_between_executions;
        this.burstCant = burstCant;
    }
    public void Configure_Callbacks(Action Execute, Action OnFinish)
    {
        this.Execute = Execute;
        this.OnFinish = OnFinish;
    }
    public void Play()
    {
        exe = true;

        currentBurstCant = this.burstCant;
        timer = this.time_space_between_executions;
    }
    public void Stop()
    {
        exe = false;

        currentBurstCant = this.burstCant;
        timer = this.time_space_between_executions;
    }

    public void Tick(float DeltaTime)
    {
        if (exe)
        {
            if (timer > 0)
            {
                timer = timer - 1 * DeltaTime;
            }
            else
            {
                if (currentBurstCant > 0)
                {
                    Execute.Invoke();
                    currentBurstCant--;
                }
                else
                {
                    OnFinish.Invoke();
                    currentBurstCant = burstCant;
                    exe = false;
                }

                timer = time_space_between_executions;
            }
        }
    }
}
