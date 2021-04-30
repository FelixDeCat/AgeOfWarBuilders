using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform from;
    public Transform to;

    public Enemy model;
    public Transform parent;

    public List<Enemy> spawner;

    Vector3 RandomPos()
    {
        return new Vector3(Random.Range(from.transform.position.x, to.transform.position.x), 
            Random.Range(from.transform.position.y, to.transform.position.y),
            Random.Range(from.transform.position.z, to.transform.position.z));
    }

    private void Start()
    {
        for (int i = 0; i < spawner.Count; i++)
        {
            spawner[i].CallbackOnDeath(Respawn);
        }
    }


    public void Respawn(Enemy enemy)
    {
        enemy.Respawn();
        enemy.transform.position = RandomPos();
    }
}
