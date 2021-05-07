using UnityEngine;
using System;

public class PlayerComponent : MonoBehaviour
{
    bool IsInitialized = false;
    bool IsOn = false;
    protected bool CanUseThisComponent;

    Transform ownerTransform;
    protected Transform GetOwnerTransform => ownerTransform;

    public PlayerComponent AddOwnerTransform(Transform trans)
    {
        ownerTransform = trans;
        return this;
    }

    public void Initialize(bool turnOn)
    {
        if (!IsInitialized)
        {
            IsInitialized = true;
            OnInitialized();
            if(turnOn) On();
        }
        else
        {
            throw new System.Exception("Estan Intentando Inicializarme otra vez");
        }
    }
    public void Tick(float DeltaTime)
    {
        if (CanUseThisComponent)
        {
            if (IsInitialized)
            {
                OnTick(DeltaTime);
            }
        }
    }
    public void On()
    {
        if (!IsOn)
        {
            OnTurnOn();
            IsOn = true;
            CanUseThisComponent = true;
        }
        else
        {
            throw new System.Exception("Me estan queriendo prender ["+ gameObject.name +"], pero ya estoy prendido");
        }
    }
    public void Off()
    {
        if (IsOn)
        {
            OnTurnOff();
            IsOn = false;
            CanUseThisComponent = false;
        }
        else
        {
            throw new System.Exception("Me estan queriendo apagar [" + gameObject.name + "], pero ya estoy apagado");
        }
    }

    protected virtual void OnInitialized() { }
    protected virtual void OnTick(float DeltaTime) { }
    protected virtual void OnTurnOn() { }
    protected virtual void OnTurnOff() { }
    
}
