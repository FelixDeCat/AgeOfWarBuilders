﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerEntity : LivingEntity
{
    public ObserverQuery query;

    public Enemy currentEnemy;

    public float Time_To_Select_Target = 1f;
    float timer;

    public Transform shootPoint;
    public GameObject bullet_Model;


    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);

        if (timer < Time_To_Select_Target)
        {
            timer = timer + 1 * DeltaTime;
        }
        else
        {
            timer = 0;

            try
            {
                //Le pregunto a la grilla y le pido el enemigo mas cercano
                currentEnemy = query
                    .Query()
                    .Cast<Enemy>()
                    .OrderBy(x => (transform.position - x.transform.position).sqrMagnitude)
                    .First();

                StartCoroutine(Shoot(currentEnemy));
            }
            catch (System.InvalidOperationException)
            {

            }
        }

    }

    IEnumerator Shoot(Enemy enemy)
    {
        yield return new WaitForEndOfFrame();
        GameObject bullet = GameObject.Instantiate(bullet_Model, shootPoint.position, shootPoint.rotation);
        bullet.GetComponent<Bullet>().Configure((enemy.transform.position+Vector3.up) - shootPoint.position, this.gameObject);
    }

}