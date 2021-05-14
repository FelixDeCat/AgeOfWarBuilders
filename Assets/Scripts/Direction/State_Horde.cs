using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Horde : StateElement
{
    public EnemySpawner[] spawners;

    protected override void OnBegin()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Begin();
        }
    }
    protected override void OnEnd() { }
    protected override void OnForceFinish() { }
    protected override void OnTick(float DeltaTime) { }
}
