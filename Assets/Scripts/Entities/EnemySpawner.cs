using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform from;
    public Transform to;

    public Enemy model;
    public Transform parent;

    public int cant = 5;
    //public List<Enemy> spawner;

    Vector3 RandomPos()
    {
        return new Vector3(Random.Range(from.transform.position.x, to.transform.position.x), 
            Random.Range(from.transform.position.y, to.transform.position.y),
            Random.Range(from.transform.position.z, to.transform.position.z));
    }

    private void Start()
    {
        PlayObject_PoolManager.instance.Feed(model, parent);

        for (int i = 0; i < cant; i++)
        {
            var enem = (Enemy)PlayObject_PoolManager.instance.Get(model.type, RandomPos(), transform.eulerAngles);
            enem.CallbackOnDeath(DeathEnemy);
        }
    }

    public void DeathEnemy(Enemy enemy)
    {
        PlayObject_PoolManager.instance.Return(enemy);
        Respawn();
    }
    void Respawn()
    {
        var enem = (Enemy)PlayObject_PoolManager.instance.Get(model.type, RandomPos(), transform.eulerAngles);
    }
}
