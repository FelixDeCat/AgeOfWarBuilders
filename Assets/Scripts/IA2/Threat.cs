using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Threat : MonoBehaviour
{
    public TextMeshPro debug_calculate_thread;

    public GridComponent myGridComponent;

    public int myThreatLevel;

    float timer;

    public void Tick(float DeltaTime)
    {
        if (timer < 1f) timer = timer + 1 * DeltaTime;
        else { timer = 0; myGridComponent.Grid_RefreshComponent(); }
    }

    public void Initialize(Transform target = null)
    {
        myGridComponent.Grid_Initialize(target ? target.gameObject : this.gameObject);
        myGridComponent.Grid_Rise();
    }
    public void Deinitialize()
    {
        myGridComponent.Grid_Deinitialize();
    }

    public void Death() => myGridComponent.Grid_Death();
    public void Rise() => myGridComponent.Grid_Rise();


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
        if(debug_calculate_thread) debug_calculate_thread.text = "Thr:" + myThreatLevel + " Mult:" + multi + " Res:" + res;
        return res;
    }
}
