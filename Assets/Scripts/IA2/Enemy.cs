using System;
using UnityEngine;
using AgeOfWarBuilders.Entities;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using AgeOfWarBuilders.Global;

public class Enemy : LivingEntity, IGridEntity
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

    public int damage = 5;

    ObserverQuery observer;
    Enemy_Zombie_View view;
    Transform playerpos;
    ThreatReceptor threatReceptor;

    float timer_recalculate;
    float time_to_recalculate = 0.5f;

    public LayerMask mypartersLayer;
    public float partner_detection_radius = 5f;


    

    protected override void OnInitialize()
    {

        // if (colorDebug == null) colorDebug = GetComponent<MeshChangeColorCollection>();
        base.OnInitialize();
        //colorDebug.Change(Color.cyan);
        view = GetComponent<Enemy_Zombie_View>();
        threatReceptor = GetComponent<ThreatReceptor>();
        Resurrect();
        view.Anim_Death(false);
        isAlive = true;
        myRig.isKinematic = true;
        myRig.detectCollisions = true;
        SpatialGrid.instance.AddEntityToGrid(this);
        playerpos = Main.instance.player.transform;

        rig_path_finder.AddCallback_OnBeginMove(PathFinderBeginMove);
        rig_path_finder.AddCallback_OnEndMove(PathFinderEndMove);

        //OnMove.Invoke(this);
    }
    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
        //colorDebug.Change(Color.magenta);
        SpatialGrid.instance.RemoveEntityToGrid(this);
    }

    Action<Enemy> CbkOnDeath;
    public void CallbackOnDeath(Action<Enemy> callback)
    {
        CbkOnDeath = callback;
    }

    LivingEntity currentarget = null;
    protected override void OnTick(float DeltaTime)
    {
        if (!isAlive) return;

        Update_TargetRecalculation();

        /*LivingEntity currentarget;
         currentarget = threatReceptor.Threats
          //  .Where(x => x.GetComponent<PlayerModel>() || x.GetComponent<Center>())
            .OfType<LivingEntity>()
            .FirstOrDefault();*/

        //if (threatReceptor.Threats.Length > 0)
        //    currentarget = threatReceptor.Threats[0].GetComponent<LivingEntity>();

        if (threatReceptor.Threats.Length > 0)
            currentarget = threatReceptor.Threats[0].GetComponent<LivingEntity>();

        Vector3 dir_to_target;
        float distance;

        #region Obtencion del target
        if (currentarget != null)
        {
            GoToPosition(currentarget.transform.position);
            dir_to_target = currentarget.transform.position - this.transform.position;
            distance = Vector3.Distance(currentarget.transform.position, this.transform.position);
        }
        else
        {
            Debug.LogWarning("Current Target es null");
            if (timer_recalculate < time_to_recalculate) timer_recalculate = timer_recalculate + 1 * DeltaTime;
            else
            {
                timer_recalculate = 0;
                if (!GoToPosition(Main.instance.player.transform.position))
                {
                    GoToPosition(Main.MyBasePosition);
                }
            }

            dir_to_target = playerpos.position - this.transform.position;
            distance = Vector3.Distance(playerpos.position, this.transform.position);
        }
        #endregion

        #region Persecucion y destruccion del target
        if (distance < 10)
        {
            CanNotWalk = true;
            dir_to_target.Normalize();

            if (distance <= rig_path_finder.distance_to_close/2)
            {
                view.Anim_Run(false);
                this.transform.forward = Vector3.Lerp(transform.forward, new Vector3(dir_to_target.x, 0, dir_to_target.z), DeltaTime * rig_path_finder.forwardspeed);
                Update_Attack();
            }
            else
            {
                view.Anim_Run(true);
                this.transform.forward = Vector3.Lerp(transform.forward, new Vector3(dir_to_target.x, 0, dir_to_target.z), DeltaTime * rig_path_finder.forwardspeed);
                myRig.isKinematic = false;
                myRig.velocity = transform.forward + dir_to_target * rig_path_finder.movement_speed * DeltaTime;
            }
        }
        else
        {
            CanNotWalk = false;
        }
        #endregion

        //esto esta despues porque aca se va a ejecutar el AStar
        base.OnTick(DeltaTime);

        //este on move es para la grid
        OnMove?.Invoke(this);

        if (PlayerController.DEBUG_PRESS_T)
        {
            GoToPosition(Main.Player.transform.position);
        }
    }

    void PathFinderBeginMove()
    {
        view.Anim_Run(true);
    }
    void PathFinderEndMove()
    {
        view.Anim_Run(false);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        //colorDebug.Change(Color.black);
        myRig.isKinematic = true;
        myRig.velocity = new Vector3(0, 0, 0);
        myRig.detectCollisions = false;
        isAlive = false;
        OnMove.Invoke(this);
        view.Anim_Death(true);
        CbkOnDeath.Invoke(this);
    }
    public void UEVENT_OnDeathFinish()
    {
        SpatialGrid.instance.RemoveEntityToGrid(this);
    }

    void Update_TargetRecalculation()
    {

    }

    void Update_Attack()
    {
        view.Anim_Attack();
    }

    #region Get Partners para Queries
    public IEnumerable<Enemy> GetPartners()
    {
        return Physics
            .OverlapSphere(this.transform.position, 5f, mypartersLayer)
            .Select(x => x.GetComponent<Enemy>());
    }

    public int GetPartnersLegth()
    {
        return Physics
            .OverlapSphere(this.transform.position, 5f, mypartersLayer).Length;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, partner_detection_radius);
    }
    #endregion

    public void UEVENT_OnAttack()
    {
        if (!currentarget) throw new Exception("La Entity es null");
        float distance = Vector3.Distance(currentarget.transform.position, this.transform.position);
        if (distance <= rig_path_finder.distance_to_close)
        {
            currentarget.ReceiveDamage(damage);
        }
    }

    protected override void Feedback_ReceiveDamage()
    {
        base.Feedback_ReceiveDamage();
        view.Particle_GetPhysicalDamage();
        view.Sound_GetPhysicalDamage();
    }
}