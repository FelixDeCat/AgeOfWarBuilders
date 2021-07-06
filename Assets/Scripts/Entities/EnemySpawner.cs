using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public Transform from;
    public Transform to;

    public Enemy[] model;
    public Transform parent;

    [SerializeField] bool use_sequence = false;
    [SerializeField] string sequence = "0,0,0,1,0,0,1"; //solo un ejemplo
    int[] translated_sequence = new int[0];
    int curretindex;

    public Text debug;

    [SerializeField] BurstExecuter burst;

    Action<EnemySpawner> OnAllEnemiesDeath = delegate { };

    int deathEnemies = 0;

    bool allEnemiesISDeath = false;
    public bool AllEnemiesIsDeath => allEnemiesISDeath;

    public bool Loop_On_Dead;

    HashSet<Enemy> enemies = new HashSet<Enemy>();

    public void Initialize(Action<EnemySpawner> OnAllEnemiesDeath)
    {
        curretindex = 0;
        Reset();
        for (int i = 0; i < model.Length; i++)
        {
            PlayObject_PoolManager.instance.FeedDatabase(model[i], parent);
        }
        
        burst.Configure_Callbacks(OnBurstExecute, OnBurstFinish);

        if (use_sequence)
        {
            string[] rawseq = sequence.Split(',');
            translated_sequence = new int[rawseq.Length];
            for (int i = 0; i < rawseq.Length; i++) { translated_sequence[i] = int.Parse(rawseq[i]); }
            burst.Configure_Basics(translated_sequence.Length);
        }

        this.OnAllEnemiesDeath = OnAllEnemiesDeath;
    }
    public void Deinitialize() { burst.Stop(); deathEnemies = 0; allEnemiesISDeath = false; }
    public void Begin() { burst.Play(); if(debug) debug.text = deathEnemies + "/" + burst.BurstCant; }
    public void Begin(string seq)
    {
        curretindex = 0;
        if (use_sequence)
        {
            string[] rawseq = seq.Split(',');
            translated_sequence = new int[rawseq.Length];
            for (int i = 0; i < rawseq.Length; i++) { translated_sequence[i] = int.Parse(rawseq[i]); }
            burst.Configure_Basics(translated_sequence.Length);
        }

        burst.Play();
        if (debug) debug.text = deathEnemies + "/" + burst.BurstCant;
    }

    void OnBurstFinish() => OnAllEnemiesDeath.Invoke(this);
    void OnBurstExecute() => Spawn().CallbackOnDeath(DeathEnemy);

    private void Reset()
    {
        allEnemiesISDeath = false;
        deathEnemies = 0;
    }

    private void Update()
    {
        burst.Tick(Time.deltaTime);
    }

    public void DeathEnemy(Enemy enemy)
    {
        deathEnemies++;
        if (debug) debug.text = deathEnemies + "/" + burst.BurstCant;

        if (deathEnemies >= burst.BurstCant)
        {
            allEnemiesISDeath = true;
            OnAllEnemiesDeath.Invoke(this);
            curretindex = 0;
        }

        PlayObject_PoolManager.instance.Return(enemy);

        if (Loop_On_Dead)
        {
            Spawn();
        }
    }

    #region [SHORTS]
    Enemy Spawn()
    {
        var e = (Enemy)PlayObject_PoolManager.instance.Get(model[translated_sequence[curretindex]].type, RandomPos(), transform.eulerAngles);
        curretindex++;
        return e;
    }
    Vector3 RandomPos()
    {
        return new Vector3(UnityEngine.Random.Range(from.transform.position.x, to.transform.position.x),
            UnityEngine.Random.Range(from.transform.position.y, to.transform.position.y),
            UnityEngine.Random.Range(from.transform.position.z, to.transform.position.z));
    }
    #endregion

    #region [GIZMOS]
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
    #endregion
}
