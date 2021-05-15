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
    protected override void OnEnd()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Stop();
        }
    }
    protected override void OnForceFinish() { }
    protected override void OnTick(float DeltaTime)
    {
        Update_CheckIfHordeIsFinish();
    }

    void Update_CheckIfHordeIsFinish()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].IsBegined)
            {
                if (!spawners[i].AllEnemiesIsDeath)
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
        ForceFinish();
    }
}
