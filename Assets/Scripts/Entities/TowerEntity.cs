using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TowerEntity : LivingEntity
{
    public Enemy currentEnemy;

    public float Time_To_Select_Target = 1f;
    protected float timer;

    public static float global_speed_tower_multiplier = 1; 

    public Transform shootPoint;

    ObserverQuery observer_query;
    protected SquareQuery square_query;

    NodeDumper dumper;
    Vector3 buildpos;

    Threat myThread;

    public GridComponent myGridCompEntity;

    public bool UseBomb;
    bool BombCreated;
    Vector3 BombPosition;

    [SerializeField] AudioClip ShootSound;
    [SerializeField] AudioClip TakeDamage;
    [SerializeField] AudioClip DestroySound;

    [SerializeField] ParticleSystem hitTower;
    protected virtual void Start()
    {
        AudioManager.instance.GetSoundPool(ShootSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, ShootSound);
        AudioManager.instance.GetSoundPool(TakeDamage.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, TakeDamage);
        AudioManager.instance.GetSoundPool(DestroySound.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, DestroySound);
    }
    void PlayClip_ShootSound()
    {
        AudioManager.instance.PlaySound(ShootSound.name, transform);
    }
    void PlayClip_TakeDadmage()
    {
        hitTower.Play();
        AudioManager.instance.PlaySound(TakeDamage.name, transform);
    }
    void PlayClip_Destroy()
    {
        AudioManager.instance.PlaySound(DestroySound.name, transform);
    }
    bool alreadyAdded = false;
    #region Enter & Exit
    protected override void OnInitialize()
    {
        base.OnInitialize();

        myThread = GetComponent<Threat>();
        myThread.Initialize(type);

        myGridCompEntity.Grid_Initialize(this.gameObject);
        myGridCompEntity.Grid_Rise();

        observer_query = GetComponentInChildren<ObserverQuery>();
        observer_query?.Configure(this.transform);
        square_query = GetComponentInChildren<SquareQuery>();
        square_query?.Configure(this.transform);

        dumper = GetComponentInChildren<NodeDumper>();
        dumper.Rise();

        TryToAddLife();

        buildpos = transform.position;
    }
    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
        myGridCompEntity.Grid_Deinitialize();
        alreadyAdded = false;
        myThread.Deinitialize();
    }
    #endregion

    public void TryToAddLife()
    {
        if (BuyItem_LifeTowers.instance)
        {
            if (BuyItem_LifeTowers.instance.isActive && !alreadyAdded)
            {
                Add_Life(BuyItem_LifeTowers.instance.cantToAdd);
                alreadyAdded = true;
            }
        }
    }

    #region [TICK]
    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);
        myGridCompEntity.Grid_RefreshComponent();
    }
    #endregion

    #region [LOGIC] Death
    Action<TowerEntity, Vector3> DeathCallback = delegate { };
    public void CallbackOnDeath(Action<TowerEntity, Vector3> _onDeath)
    {
        DeathCallback = _onDeath;
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        myThread.Death();
        myGridCompEntity.Grid_Death();

        Invoke("Wait", 0.1f);
        PlayClip_Destroy();

    }
    protected override void Feedback_ReceiveDamage()
    {
        PlayClip_TakeDadmage();
        base.Feedback_ReceiveDamage();
    }
    void Wait()
    {
        dumper.Death();
        DeathCallback?.Invoke(this, buildpos);
        transform.position = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        Destroy(this.gameObject, 0.2f);
    }
    #endregion

    //IEnumerable<Enemy> partners;

    protected virtual void LateUpdate()
    {
        if (timer < Time_To_Select_Target)
        {
            if (BuyItem_FastTower.instance)
            {
                timer = timer + (BuyItem_FastTower.instance.isActive ? BuyItem_FastTower.instance.global_speed_modifier : 1f) * Time.deltaTime;
            }
            else
            {
                timer = timer + 1 * Time.deltaTime;
            }
            
        }
        else
        {
            timer = 0;

            if (UseBomb)
            {
                var currentEnemy = square_query
                .Query() //IA2-P2 [SpatialGrid - Bombs]
                .OfType<GridComponent>()
                .Where(x => x.GetComponent<Enemy>())
                .Select(x => x.GetComponent<Enemy>())
                .OrderByDescending(x => x.GetPartnersLegth()) //IA2-P3 [OrderBy]
                .FirstOrDefault();

                if (currentEnemy != null)
                {
                var pointToShootBomb = currentEnemy.GetPartners().Aggregate(Tuple.Create(0, Vector3.zero), //IA2-P3 [Aggregate]
                    (acum, enemy) =>
                    {
                        //acumulo para el Legth
                        var num = acum.Item1;
                        num++;

                        //acumulo para dividir por el Legth
                        var pos = acum.Item2;
                        pos += enemy.transform.position;

                        return Tuple.Create(num, pos);
                    }
                );

                    BombCreated = true;
                    //obtengo el punto central de todos los vectores
                    BombPosition = pointToShootBomb.Item2 / pointToShootBomb.Item1;

                    var dir = (BombPosition - shootPoint.position).normalized;
                    Bomb_PoolManager.instance.Shoot(shootPoint.position, dir, 2f, 25f);
                    PlayClip_ShootSound();
                }

                // 


            }
            else
            {
                //Le pregunto a la grilla y le pido el enemigo mas cercano
                currentEnemy = observer_query
                    .Query() //IA2-P2 [SpatialGrid - Arrows]
                    .OfType<GridComponent>()
                    .Where(x => x.Grid_Object.GetComponent<Enemy>()) //IA2-P3 [Where]
                    .Select(x => x.Grid_Object.GetComponent<Enemy>()) //IA2-P3 [Select]
                    .OrderBy(x => (transform.position - x.transform.position).sqrMagnitude) //IA2-P3 [OrderBy]
                    .FirstOrDefault();

                if (currentEnemy)
                {
                    var pos = shootPoint.transform.position;
                    var dir = (currentEnemy.transform.position + Vector3.up - shootPoint.position).normalized;
                    Bullet_PoolManager.instance.Shoot(pos, dir);
                    PlayClip_ShootSound();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        //if (BombCreated)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(BombPosition, 2f);

        //    foreach (var p in partners)
        //    {
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawLine(p.transform.position + Vector3.up, BombPosition + Vector3.up);
        //    }
        //}
    }
}
