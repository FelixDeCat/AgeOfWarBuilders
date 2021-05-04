using System.Collections;
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
    }
    private void LateUpdate()
    {
        if (timer < Time_To_Select_Target)
        {
            timer = timer + 1 * Time.deltaTime;
        }
        else
        {
            timer = 0;

            //Le pregunto a la grilla y le pido el enemigo mas cercano
            currentEnemy = query
                .Query()
                .Cast<Enemy>()
                .OrderBy(x => (transform.position - x.transform.position).sqrMagnitude)
                .FirstOrDefault();

            if (currentEnemy)
            {
                var pos = shootPoint.transform.position;
                var dir = (currentEnemy.transform.position + Vector3.up - shootPoint.position).normalized;
                Bullet_PoolManager.instance.Shoot(pos,dir);
            }
            
        }
    }

}
