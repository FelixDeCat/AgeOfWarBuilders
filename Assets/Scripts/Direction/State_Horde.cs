using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class State_Horde : StateElement
{
    public EnemySpawner[] spawners;

    public Tuple<EnemySpawner, int, bool>[] spawners_data = new Tuple<EnemySpawner, int, bool>[0];

    public string sequence;


    protected override void OnInitialize()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Initialize(OnSpawnerFinish);
        }
    }
    protected override void OnBegin()
    {
        Main.PlayClip_Horde();
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Begin(sequence);
        }
        UI_StateTimer.instance.Open();
        UI_StateTimer.Refresh(stateName);
    }
    protected override void OnEnd()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Deinitialize();
        }
    }

    public void OnSpawnerFinish(EnemySpawner spawner)
    {
        //actualizo este spawner... UI y esas cosas

        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].AllEnemiesIsDeath)
            {
                //si estan todos muertos, continuo el for
                continue;
            }
            else
            {
                //si con solo uno que no esten todos muertos, corto la ejecucion
                return;
            }
        }

        //cuando esten todos muertos
        ForceFinish();
    }

    protected override void OnForceFinish() { }
    protected override void OnTick(float DeltaTime) { }

}
