using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class VigilancePoint : TowerEntity
{
    [Header("VigilancePoint")]
    public bool inDanger;
    [SerializeField] AudioClip DingSound;

    protected override void Start()
    {
        base.Start();
        AudioManager.instance.GetSoundPool(DingSound.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, DingSound);
    }
    public void PlayClip_DingSound()
    {
        AudioManager.instance.PlaySound(DingSound.name, transform);
    }

    protected override void LateUpdate()
    {
        if (timer < Time_To_Select_Target)
        {
            timer = timer + 1 * Time.deltaTime;
        }
        else
        {
            timer = 0;

            var col = square_query
                .Query()
                .OfType<GridComponent>();

            var enemyCol  = col.Where(x => x.GetComponent<Enemy>());
            var VillagerCol = col
                .Where(x => x.GetComponent<Villager>())
                .Select(x => x.GetComponent<Villager>());

            Debug.Log("Se esta ejecutando: tengo: Enemigos: " + enemyCol.Count() + " y villagers: " + VillagerCol.Count());

            if (enemyCol.Count() > 0)
            {
                foreach (var v in VillagerCol)
                {
                    v.inDanger = true;
                    v.Replan();
                }
            }
            else
            {
                foreach (var v in VillagerCol)
                {
                    v.inDanger = false;
                    v.Replan();
                }
            }
        }
    }
}
