using System;
using UnityEngine;
using AgeOfWarBuilders.Entities;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using AgeOfWarBuilders.Global;
using Tools.StateMachine;

public class Enemy : LivingEntity
{
    [Header("Enemy Things")]
    public int damage = 5;

    public GridComponent MyGridComponentEntity;

    bool isAlive;

    public enum EnemyInputs { AWAKE, IDLE, BEGIN_ATTACK, ATTACK, GO_TO_POS, DIE, DISABLE, TAKE_DAMAGE, PARRIED, CHASING, SPECIAL_ATTACK };
    public EventStateMachine<EnemyInputs> sm;

    Enemy_Zombie_View view;
    Transform playerpos;
    ThreatReceptor threatReceptor;
    AnimEvent myAnimEvent;

    SmoothLookAt SmoothLookAt;

    [System.Serializable]
    class States
    {
        [SerializeField] internal EnemyBase_Idle idle_state;
    }
    [SerializeField] States states;


    float timer_recalculate;
    float time_to_recalculate = 0.5f;

    public LayerMask mypartersLayer;
    public float partner_detection_radius = 5f;

    protected override void OnInitialize()
    {
        isAlive = true;
        // if (colorDebug == null) colorDebug = GetComponent<MeshChangeColorCollection>();
        base.OnInitialize();
        //colorDebug.Change(Color.cyan);
        view = GetComponent<Enemy_Zombie_View>();
        threatReceptor = GetComponent<ThreatReceptor>();
        Resurrect();
        view.Anim_Death(false);

        myRig.isKinematic = true;
        myRig.detectCollisions = true;
        MyGridComponentEntity.Grid_Initialize(this.gameObject);
        MyGridComponentEntity.Grid_Rise();
        playerpos = SceneReferences.Player.transform;

        SmoothLookAt = GetComponentInChildren<SmoothLookAt>();

        rig_path_finder.AddCallback_OnBeginMove(PathFinderBeginMove);
        rig_path_finder.AddCallback_OnEndMove(PathFinderEndMove);

        myAnimEvent = this.GetComponentInChildren<AnimEvent>();
        myAnimEvent.ADD_ANIM_EVENT_LISTENER("OnAttack", ANIM_EVENT_OnAttack);
        myAnimEvent.ADD_ANIM_EVENT_LISTENER("OnDeathFinish", ANIM_EVENT_OnDeathFinish);

    }

    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
        //colorDebug.Change(Color.magenta);
        MyGridComponentEntity.Grid_Deinitialize();
    }

    void DebugState(string EnemyState)
    {

    }

    void InitializeAI()
    {
        var idle = new EState<EnemyInputs>("Idle", states.idle_state);
        var Attack = new EState<EnemyInputs>("Attack");

        ConfigureState.Create(idle)
            .SetTransition(EnemyInputs.ATTACK, Attack)
            .Done();

        sm = new EventStateMachine<EnemyInputs>(idle, DebugState);
    }

    public void SendInput(EnemyInputs input)
    {
        sm.SendInput(input);
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

        var threat = threatReceptor
            .Threats
            .FirstOrDefault();

        if (threat != null)
        {
            currentarget = threat.GetComponent<LivingEntity>();
        }

        //currentarget

        //if (threatReceptor.Threats.Length > 0)
        //    currentarget = threatReceptor.Threats[0].GetComponent<LivingEntity>();

        Vector3 dir_to_target;
        float distance;

        


        #region Obtencion del target
        if (currentarget != null)
        {
            if (timer_recalculate < time_to_recalculate) timer_recalculate = timer_recalculate + 1 * DeltaTime;
            else
            {
                GoToPosition(currentarget.transform.position);
            }
           
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
                if (!GoToPosition(SceneReferences.Player.transform.position))
                {
                    GoToPosition(SceneReferences.MyBasePosition);
                }
            }

            dir_to_target = playerpos.position - this.transform.position;
            distance = Vector3.Distance(playerpos.position, this.transform.position);
        }
        #endregion

        dir_to_target.Normalize();
        SmoothLookAt.SetDirection(dir_to_target);

        #region Persecucion y destruccion del target
        if (distance < 10)
        {
            CanNotWalk = true;
            dir_to_target.Normalize();

            if (distance <= rig_path_finder.distance_to_close / 2)
            {
                view.Anim_Run(false);
                SmoothLookAt.Look(DeltaTime);
                Update_Attack();
            }
            else
            {
                view.Anim_Run(true);
                SmoothLookAt.Look(DeltaTime);
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
        MyGridComponentEntity.Grid_RefreshComponent();

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
        MyGridComponentEntity.Grid_RefreshComponent();
        view.Anim_Death(true);
        CbkOnDeath?.Invoke(this);
    }

    public void ANIM_EVENT_OnAttack()
    {
        if (!currentarget) throw new Exception("La Entity es null");
        float distance = Vector3.Distance(currentarget.transform.position, this.transform.position);
        if (distance <= rig_path_finder.distance_to_close)
        {
            currentarget.ReceiveDamage(damage);
        }
    }

    public void ANIM_EVENT_OnDeathFinish()
    {
        MyGridComponentEntity.Grid_Deinitialize();
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
            .Select(x => x.GetComponent<Enemy>()) //IA2-P3 [Select]
            .Take(3); //IA2-P3 [Take]
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



    protected override void Feedback_ReceiveDamage()
    {
        base.Feedback_ReceiveDamage();
        view.Particle_GetPhysicalDamage();
        view.Sound_GetPhysicalDamage();
    }
}