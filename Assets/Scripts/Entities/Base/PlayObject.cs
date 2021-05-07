using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Interfaces;

public enum PlayObjectType { enemy, tower }
public abstract class PlayObject : MonoBehaviour, IPausable, IResumeable, IUpdateable, IInitializable
{
    int index = -1;
    public int Index { get => index; set => index = value; }


    public PlayObjectType type;

    bool isInitialized;
    public bool IsInitialized { get => isInitialized; }

    public bool AddOnAwake = false;
    protected virtual void Awake()
    {
        if (AddOnAwake) GameLoop.AddObject(this);
    }

    //INTERFACE IMPLEMENTATION
    public void Initialize()
    {
        if (!IsInitialized)
        {
            OnInitialize();
            isInitialized = true;
        }
        else
        {
            Debug.LogWarning("Already Initizalized");
        }
    }
    public void Deinitialize() { OnDeinitialize(); isInitialized = false; }
    public void Pause() { OnPause(); }
    public void Resume() { OnResume(); }
    public void Tick(float DeltaTime) { OnTick(DeltaTime); }

    //FOR INHERITANCE
    protected virtual void OnInitialize() { }
    protected virtual void OnDeinitialize() { }
    protected virtual void OnPause() { }
    protected virtual void OnResume() { }
    protected virtual void OnTick(float DeltaTime) { }

    //THIS FUNCTIONS ARE TRIGGRED BY POOLMANAGER
    public void On() => GameLoop.AddObject(this);
    public void Off() => GameLoop.RemoveObject(this);
}