using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Threat : MonoBehaviour, IGridEntity
{
    public TextMeshPro debug_calculate_thread;

    public event Action<IGridEntity> OnMove;
    public Vector3 Position
    {
        get
        {
            if (transform != null)
                return transform.position;
            else
                return new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        }
        set => transform.position = value;
    }
    bool isAlive;
    public bool IsAlive { get => isAlive; set => isAlive = value; }

    public int myThreatLevel;

    float timer;

    public void Tick(float DeltaTime)
    {
        Debug.Log("Me estan Updateando");
        if (timer < 1f) timer = timer + 1 * DeltaTime;
        else { timer = 0; OnMove.Invoke(this); }
    }

    public void Initialize() { isAlive = true; if (SpatialGrid.instance) SpatialGrid.instance.AddEntityToGrid(this); else Invoke("RetardedInitialization", 0.1f); }
    public void Deinitialize() { isAlive = false; SpatialGrid.instance.RemoveEntityToGrid(this); }

    void RetardedInitialization()
    {
        SpatialGrid.instance.AddEntityToGrid(this);
    }

    [Header("Threads levels by distance")]
    [SerializeField] float hight_thread_distance = 6;
    [Range(0, 5)] [SerializeField] int hight_thread_multiplier = 3;

    [SerializeField] float medium_thread_distance = 12;
    [Range(0, 5)] [SerializeField] int medium_thread_multiplier = 2;

    [SerializeField] float low_thread_distance = 20;
    [Range(0, 5)] [SerializeField] int low_thread_multiplier = 1;

    public int ThreatByDistanceMultiplier(Vector3 position)
    {
        float dist = (position - transform.position).sqrMagnitude;
        int multi;

        if (dist <= hight_thread_distance) 
        {
            multi = hight_thread_multiplier;
        }
        else
        {
            if (dist > hight_thread_distance && dist <= medium_thread_distance)
            {
                multi = medium_thread_multiplier;
            }
            else
            {
                multi = low_thread_multiplier;
            }
        }
        var res = myThreatLevel* multi;
        debug_calculate_thread.text = "Thr:" + myThreatLevel + " Mult:" + multi + " Res:" + res;
        return res;
    }
}
