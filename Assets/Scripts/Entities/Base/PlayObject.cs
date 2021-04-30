using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Interfaces;

public abstract class PlayObject : MonoBehaviour, IPausable, IResumeable, IUpdateable, IInitializable
{
    int index = -1;
    public int Index { get => index; set => index = value; }

    void Awake()
    {
        GameLoop.AddObject(this);
    }

    //INTERFACE IMPLEMENTATION
    public void Initialize() { OnInitialize(); Debug.Log("Initialize PlayObject"); }
    public void Pause() { OnPause(); }
    public void Resume() { OnResume(); }
    public void Tick(float DeltaTime) { OnTick(DeltaTime); }

    //FOR INHERITANCE
    protected virtual void OnInitialize() { }
    protected virtual void OnPause() { }
    protected virtual void OnResume() { }
    protected virtual void OnTick(float DeltaTime) { }
}
