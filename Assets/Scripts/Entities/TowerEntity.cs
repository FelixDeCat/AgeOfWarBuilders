using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TowerEntity : LivingEntity, IGridEntity
{
    #region Grid Things
    public event Action<IGridEntity> OnMove;
    public Vector3 Position
    {
        get
        {
            if (transform != null)
                return transform.position;
            else
                return new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        }
        set => transform.position = value;
    }
    bool isAlive;
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    #endregion

    public Enemy currentEnemy;

    public float Time_To_Select_Target = 1f;
    float timer;

    public Transform shootPoint;

    ObserverQuery observer_query;
    SquareQuery square_query;

    public bool UseBomb;
    bool BombCreated;
    Vector3 BombPosition;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        isAlive = true;
        if (SpatialGrid.instance) SpatialGrid.instance.AddEntityToGrid(this); else Invoke("RetardedInitialization", 0.1f);

        observer_query = GetComponentInChildren<ObserverQuery>();
        observer_query?.Configure(this.transform);
        square_query = GetComponentInChildren<SquareQuery>();
        square_query?.Configure(this.transform);
    }
    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
        isAlive = false;
        SpatialGrid.instance.RemoveEntityToGrid(this);
    }

    void RetardedInitialization()
    {
        SpatialGrid.instance.AddEntityToGrid(this);
    }

    protected override void OnTick(float DeltaTime)
    {
        if (!isAlive) return;
        base.OnTick(DeltaTime);
    }
    Action<TowerEntity> DeathCallback = delegate { };
    public void CallbackOnDeath(Action<TowerEntity> _onDeath)
    {
        DeathCallback = _onDeath;

    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Invoke("Wait", 0.1f);
    }
    void Wait()
    {
        DeathCallback?.Invoke(this);
        Off();
        transform.position = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        Destroy(this.gameObject, 0.2f);
    }

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
                .OfType<Enemy>()
                .OrderByDescending(x => x.GetPartnersLegth())
                .First()
                .GetPartners();


                var pointToShootBomb = partners.Aggregate(Tuple.Create(0, Vector3.zero),
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

                ////ejemplo con frutas Agreggate
                //    string[] fruits = { "apple", "mango", "orange", "passionfruit", "grape" };
                //// Determine whether any string in the array is longer than "banana".
                //string longestName =
                //    fruits.Aggregate("banana",
                //                    (longest, next) =>
                //                        next.Length > longest.Length ? next : longest,
                //                    // Return the final result as an upper case string.
                //                    fruit => fruit.ToUpper());
            }
            else
            {
                //Le pregunto a la grilla y le pido el enemigo mas cercano
                currentEnemy = observer_query
                    .Query()
                    .OfType<Enemy>()
                    .OrderBy(x => (transform.position - x.transform.position).sqrMagnitude)
                    .FirstOrDefault();

                if (currentEnemy)
                {
                    var pos = shootPoint.transform.position;
                    var dir = (currentEnemy.transform.position + Vector3.up - shootPoint.position).normalized;
                    Bullet_PoolManager.instance.Shoot(pos, dir);
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
