using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform from;
    public Transform to;

    public Enemy model;
    public Transform parent;

    [SerializeField] BurstExecuter burst;

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

        //for (int i = 0; i < cant; i++)
        //{
        //    SpawnOneEnemy();
        //}
        
    }

    public void Begin()
    {
        burst.Begin(SpawnOneEnemy);
    }

    private void Update()
    {
        burst.Tick(Time.deltaTime);
    }

    public void SpawnOneEnemy()
    {
        var enem = (Enemy)PlayObject_PoolManager.instance.Get(model.type, RandomPos(), transform.eulerAngles);
        enem.CallbackOnDeath(DeathEnemy);
    }

    public void DeathEnemy(Enemy enemy)
    {
       /* 
        PlayObject_PoolManager.instance.Return(enemy);
        Respawn();*/
    }
    void Respawn()
    {
        var enem = (Enemy)PlayObject_PoolManager.instance.Get(model.type, RandomPos(), transform.eulerAngles);
    }

    private void OnDrawGizmos()
    {
        var vfrom = new Vector3(Mathf.Min(from.position.x, to.position.x), 0, Mathf.Min(from.position.z, to.position.z));
        var vto = new Vector3(Mathf.Max(from.position.x, to.position.x), 0, Mathf.Max(from.position.z, to.position.z));

        float horizontalDist = vto.x - vfrom.x;
        float VerticalDist = vto.z - vfrom.z;
        float pos_x_center = vfrom.x + horizontalDist / 2;
        float pos_z_center = vfrom.z + VerticalDist / 2;

        float scale_x = horizontalDist;
        float scale_z = VerticalDist;

        Gizmos.DrawWireCube(new Vector3(pos_x_center, 0, pos_z_center), new Vector3(scale_x, 0, scale_z));


    }
}
