using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TowerEntity : LivingEntity
{
    public Enemy currentEnemy;

    public float Time_To_Select_Target = 1f;
    float timer;

    public Transform shootPoint;

    ObserverQuery observer_query;
    SquareQuery square_query;

    NodeDumper dumper;

    Threat myThread;

    public GridComponent myGridCompEntity;

    public bool UseBomb;
    bool BombCreated;
    Vector3 BombPosition;

    #region Enter & Exit
    protected override void OnInitialize()
    {
        base.OnInitialize();

        myThread = GetComponent<Threat>();
        myThread.Initialize();

        myGridCompEntity.Grid_Initialize(this.gameObject);
        myGridCompEntity.Grid_Rise();

        observer_query = GetComponentInChildren<ObserverQuery>();
        observer_query?.Configure(this.transform);
        square_query = GetComponentInChildren<SquareQuery>();
        square_query?.Configure(this.transform);

        //dumper = GetComponentInChildren<NodeDumper>();
        //dumper.Rise();
    }
    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
        myGridCompEntity.Grid_Deinitialize();

        myThread.Deinitialize();
    }
    #endregion

    #region [TICK]
    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);
        //myGridCompEntity.Grid_RefreshComponent();
    }
    #endregion

    #region [LOGIC] Death
    Action<TowerEntity> DeathCallback = delegate { };
    public void CallbackOnDeath(Action<TowerEntity> _onDeath)
    {
        DeathCallback = _onDeath;
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        myThread.Death();
        myGridCompEntity.Grid_Death();

        Invoke("Wait", 0.1f);

    }
    void Wait()
    {
        //dumper.Death();
        DeathCallback?.Invoke(this);
        transform.position = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        Destroy(this.gameObject, 0.2f);
    }
    #endregion

    IEnumerable<Enemy> partners;

    private void LateUpdate()
    {
        if (timer < Time_To_Select_Target)
        {
            timer = timer + 1 * Time.deltaTime;
        }
        else
        {
            timer = 0;

            if (UseBomb)
            {
                partners = square_query
                .Query()
                .OfType<GridComponent>()
                .Select(x => x.GetComponent<Enemy>())
                .OrderByDescending(x => x.GetPartnersLegth()) //IA2-P3 [OrderBy]
                .First()
                .GetPartners();

                var pointToShootBomb = partners.Aggregate(Tuple.Create(0, Vector3.zero), //IA2-P3 [Aggregate]
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

            }
            else
            {
                //Le pregunto a la grilla y le pido el enemigo mas cercano
                currentEnemy = observer_query
                    .Query()
                    .OfType<GridComponent>()
                    .Where( x => x.Grid_Object.GetComponent<Enemy>())
                    .Select(x => x.Grid_Object.GetComponent<Enemy>())
                    .OrderBy(x => (transform.position - x.transform.position).sqrMagnitude)
                    .FirstOrDefault();

                if (currentEnemy)
                {
                    var pos = shootPoint.transform.position;
                    var dir = (currentEnemy.transform.position + Vector3.up - shootPoint.position).normalized;
                    Bullet_PoolManager.instance.Shoot(pos, dir,1,50,1);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (BombCreated)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(BombPosition, 2f);

            foreach (var p in partners)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(p.transform.position + Vector3.up, BombPosition + Vector3.up);
            }
        }
    }
}
