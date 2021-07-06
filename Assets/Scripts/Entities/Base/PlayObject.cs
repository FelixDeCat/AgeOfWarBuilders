using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Interfaces;

public enum PlayObjectType { enemy, tower_arrow, tower_bomb, enemy_secondary }
public enum spawn_logic { autonomous, belongs_to_a_pool }
public abstract class PlayObject : MonoBehaviour, IPausable, IResumeable, IUpdateable, IInitializable
{
    int index = -1;
    public int Index { get => index; set => index = value; }

    [Header("Este type es para los diccionarios de los PoolManagers")]
    public PlayObjectType type;

    //este spawn logic es para diferenciar el tipo de Inicializacion que va a tener
    // si es "autonomous", la inicializacion es automatica en el Awake
    // si es "belongs_to_a_pool", la inicializacion la hace el PoolManager
    public spawn_logic spawn_logic;

    bool isInitialized = false;
    public bool IsInitialized { get => isInitialized; }

    protected virtual void Awake()
    {
        if (spawn_logic == spawn_logic.autonomous)
        {
            Invoke("RetardedInitialization", 0.01f);
            
        }
    }
    protected virtual void OnDestroy()
    {

    }

    void RetardedInitialization()
    {
        Initialize();
    }

    //INTERFACE IMPLEMENTATION
    public void Initialize()
    {
        if (!isInitialized)
        {
            OnInitialize();
            isInitialized = true;
            GameLoop.AddObject(this);
        }
        else Debug.LogWarning("Already Initizalized");
    }
    public void DeInitialize()
    {
        if (isInitialized)
        {
            OnDeinitialize();
            isInitialized = false;
            GameLoop.RemoveObject(this);
            
        }
        else Debug.LogWarning("Already Deinitialized");
    }
    public void Pause() { OnPause(); }
    public void Resume() { OnResume(); }
    public void Tick(float DeltaTime) { OnTick(DeltaTime); }

    //FOR INHERITANCE
    protected virtual void OnInitialize() { }
    protected virtual void OnDeinitialize() { }
    protected virtual void OnPause() { }
    protected virtual void OnResume() { }
    protected virtual void OnTick(float DeltaTime) { }
}