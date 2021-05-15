using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class BurstExecuter : MonoBehaviour
{
    //por si se quiere hacer desde editor
    public UnityEvent Execute_One_Element;

    //por si se lo queremos pasar por callback
    public Action ExecuteOneElement;
    public Action OnFinishBurst;

    bool useCallback;

    bool canBegin;

    float timer;
    [SerializeField] bool UseThisEditorValues = false;
    [SerializeField] float time_space_between_executions = 0.2f;
    [SerializeField] int burstCant = 3;
    public int BurstCant => burstCant;
    int currentBurstCant;



    public void Begin(int burstCant = 3, float time_between_executions = 0.2f)
    {
        useCallback = false;
        if (!UseThisEditorValues) this.time_space_between_executions = time_between_executions;
        if (!UseThisEditorValues) this.burstCant = burstCant;
        currentBurstCant = this.burstCant;
        timer = this.time_space_between_executions;
        canBegin = true;
    }
    public void Begin(Action ExecuteOneElement, Action _OnFinishBurst, int burstCant = 3, float time_between_executions = 0.2f)
    {
        OnFinishBurst = _OnFinishBurst;
        useCallback = true;
        this.ExecuteOneElement = ExecuteOneElement;
        if (!UseThisEditorValues) this.time_space_between_executions = time_between_executions;
        if (!UseThisEditorValues) this.burstCant = burstCant;
        currentBurstCant = this.burstCant;
        timer = this.time_space_between_executions;
        canBegin = true;
    }
    public void Stop()
    {
        canBegin = false;
    }

    public void Tick(float DeltaTime)
    {
        if (canBegin)
        {
            if (timer > 0)
            {
                timer = timer - 1 * DeltaTime;
            }
            else
            {
                if (currentBurstCant > 0)
                {
                    if (useCallback) ExecuteOneElement.Invoke();
                    else Execute_One_Element.Invoke();
                    currentBurstCant--;
                }
                else
                {
                    OnFinishBurst?.Invoke();
                    currentBurstCant = burstCant;
                    canBegin = false;
                }

                timer = time_space_between_executions;
            }
        }
    }
}
