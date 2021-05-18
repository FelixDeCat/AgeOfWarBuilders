using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateElement : MonoBehaviour
{
    [SerializeField] protected string stateName;
    bool isRunning;
    public bool IsRunning { get => isRunning; }
    Action FinishCallback;

    public void Initialize(Action onFinish)
    {
        FinishCallback = onFinish;
        OnInitialize();
    }

    public void BeginExecution()
    {
        OnBegin();
        isRunning = true;
    }

    public void Tick(float DeltaTime)
    {
        if (isRunning)
        {
            OnTick(DeltaTime);
        }
    }
    public void End()
    {
        FinishCallback = delegate { };
        isRunning = false;
        OnEnd();
    }

    public void ForceFinish()
    {
        OnForceFinish();
        FinishCallback.Invoke();
    }

    protected abstract void OnInitialize();
    protected abstract void OnBegin();
    protected abstract void OnTick(float DeltaTime);
    protected abstract void OnEnd();
    protected abstract void OnForceFinish();
}
