using System;
using UnityEngine;
using AgeOfWarBuilders.Entities;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Tools.StateMachine;
using TMPro;

public class Enemy : LivingEntity
{
    [Header("Enemy Things")]
    public int damage = 5;

    public GridComponent MyGridComponentEntity;

    bool isAlive;

    [SerializeField] bool use_protection = false;

    const float FAR_DISTANCE = 10;

    public enum EnemyInputs
    {
        ImRelax,
        MyObjetiveIsFar,
        MyObjetiveIsNear,
        ILostMyObjetive,
        ICanAttack,
        IAmDeath,
        ResurrectMe,
        MyObjetiveIsDead,
        MyAttackIsFinish
    };
    public EventStateMachine<EnemyInputs> sm;

    [System.NonSerialized] public Enemy_Zombie_View view;
    [SerializeField] public AnimEvent myAnimEvent;
    [System.NonSerialized] public FindTarget targetFinder;
    [System.NonSerialized] public FollowComponent follow_component;
    Transform playerpos;
    ThreatReceptor threatReceptor;
    [System.NonSerialized] public SmoothLookAt smoothLookAt;


    [System.Serializable]
    class States
    {
        [SerializeField] internal EnemyBase_Rising rising;
        [SerializeField] internal EnemyBase_Idle idle;
        [SerializeField] internal EnemyBase_Chasing chasing;
        [SerializeField] internal EnemyBase_Fighting fighting;
        [SerializeField] internal EnemyBase_Attacking attacking;
        [SerializeField] internal EnemyBase_Dying dying;
    }
    [SerializeField] States states;
    [SerializeField] TextMeshProUGUI textdebug_states;


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
        targetFinder = GetComponent<FindTarget>();
        follow_component = GetComponentInChildren<FollowComponent>();
        Resurrect();

        if (use_protection)
        {
            var protection = GetComponent<SimpleProtection>();
            if (protection)
            {
                protection.Initialize();
                Protection = protection.TakeDamage;
            }
        }

        myRig.isKinematic = true;
        myRig.detectCollisions = true;
        MyGridComponentEntity.Grid_Initialize(this.gameObject);
        MyGridComponentEntity.Grid_Rise();
        playerpos = SceneReferences.Player.transform;

        smoothLookAt = GetComponentInChildren<SmoothLookAt>();

        //myAnimEvent = this.GetComponentInChildren<AnimEvent>(false);
        //myAnimEvent.ADD_ANIM_EVENT_LISTENER("OnAttack", ANIM_EVENT_OnAttack);
        myAnimEvent.ADD_ANIM_EVENT_LISTENER("OnDeathFinish", ANIM_EVENT_OnDeathFinish);
        myAnimEvent.ADD_ANIM_EVENT_LISTENER("OnFootStep", view.Play_Clip_Walk);

        InitializeAI();

    }

    private void Reset()
    {
        
    }

    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();

        myAnimEvent.REMOVE_ANIM_EVENT_LISTENER("OnDeathFinish", ANIM_EVENT_OnDeathFinish);
        myAnimEvent.REMOVE_ANIM_EVENT_LISTENER("OnFootStep", view.Play_Clip_Walk);

        //colorDebug.Change(Color.magenta);
        MyGridComponentEntity.Grid_Deinitialize();
    }

    void DebugState(string EnemyState)
    {
        textdebug_states.text = EnemyState;
    }

    void InitializeAI()
    {
        //IA2-P1 [StateMachine]

        var rising = new EState<EnemyInputs>("rising", states.rising);
        var idle = new EState<EnemyInputs>("idle", states.idle);
        var chasing = new EState<EnemyInputs>("chasing", states.chasing);
        var fighting = new EState<EnemyInputs>("fighting", states.fighting);
        var attacking = new EState<EnemyInputs>("attacking", states.attacking);
        var dying = new EState<EnemyInputs>("dying", states.dying);

        ConfigureState.Create(rising)
            .SetTransition(EnemyInputs.ImRelax, idle)
            .SetTransition(EnemyInputs.MyObjetiveIsFar, chasing)
            .SetTransition(EnemyInputs.MyObjetiveIsNear, fighting)
            .SetTransition(EnemyInputs.IAmDeath, dying)
            .Done();

        ConfigureState.Create(idle)
            .SetTransition(EnemyInputs.MyObjetiveIsFar, chasing)
            .SetTransition(EnemyInputs.MyObjetiveIsNear, fighting)
            .SetTransition(EnemyInputs.IAmDeath, dying)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(EnemyInputs.ImRelax, idle)
            .SetTransition(EnemyInputs.MyObjetiveIsNear, fighting)
            .SetTransition(EnemyInputs.IAmDeath, dying)
            .Done();

        ConfigureState.Create(fighting)
            .SetTransition(EnemyInputs.ImRelax, idle)
            .SetTransition(EnemyInputs.MyObjetiveIsFar, chasing)
            .SetTransition(EnemyInputs.ICanAttack, attacking)
            .SetTransition(EnemyInputs.MyObjetiveIsDead, idle)
            .SetTransition(EnemyInputs.IAmDeath, dying)
            .Done();

        ConfigureState.Create(attacking)
            .SetTransition(EnemyInputs.MyAttackIsFinish, fighting)
            .SetTransition(EnemyInputs.MyObjetiveIsDead, fighting)
            .SetTransition(EnemyInputs.IAmDeath, dying)
            .Done();

        ConfigureState.Create(dying)
            .SetTransition(EnemyInputs.ResurrectMe, rising)
            .Done();

        states.rising.Configure(SendInput, this);
        states.idle.Configure(SendInput, this);
        states.chasing.Configure(SendInput, this);
        states.fighting.Configure(SendInput, this);
        states.attacking.Configure(SendInput, this);
        states.dying.Configure(SendInput, this);


        sm = new EventStateMachine<EnemyInputs>(rising, DebugState);
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

        

        //smoothLookAt.SetDirection(targetFinder.DirectionToTarget);

        //if (QUERY_TargetIsFar)
        //{
        //    GoToPositionWithPathFinder(targetFinder.Target.transform.position);
        //}
        //else
        //{
        //    CanNotUsePathFinder = true;

        //    if (QUERY_IsTooClose)
        //    {
        //        view.Anim_Run(false);
        //        smoothLookAt.Look(DeltaTime);
        //        Update_Attack();
        //    }
        //    else
        //    {
        //        view.Anim_Run(true);
        //        smoothLookAt.Look(DeltaTime);
        //        follow_component.Tick_Follow(DeltaTime, targetFinder.DirectionToTarget);
        //    }
        //}

        //esto esta despues porque aca se va a ejecutar el AStar
        base.OnTick(DeltaTime);

        targetFinder.TickFinding();

        sm.Update();

        //este on move es para la grid
        MyGridComponentEntity.Grid_RefreshComponent();

    }

    public bool QUERY_TargetIsFar => targetFinder.DistanceToTarget > FAR_DISTANCE;
    public bool QUERY_IsTooClose => targetFinder.DistanceToTarget <= rig_path_finder.distance_to_close / 2;

    protected override void OnDeath()
    {
        base.OnDeath();
        SendInput(EnemyInputs.IAmDeath);
        //colorDebug.Change(Color.black);
        myRig.isKinematic = true;
        myRig.velocity = new Vector3(0, 0, 0);
        myRig.detectCollisions = false;
        view.Particle_GetPhysicalDamage();
        isAlive = false;
        MyGridComponentEntity.Grid_RefreshComponent();
    }

    public void ANIM_EVENT_OnDeathFinish()
    {
        Debug.Log("Animevent_deathFinish");
        CbkOnDeath?.Invoke(this);
        MyGridComponentEntity.Grid_Deinitialize();
    }

    //public void ANIM_EVENT_OnAttack()
    //{
    //    if (!currentarget) throw new Exception("La Entity es null");
    //    float distance = Vector3.Distance(currentarget.transform.position, this.transform.position);
    //    if (distance <= rig_path_finder.distance_to_close)
    //    {
    //        currentarget.ReceiveDamage(damage);
    //    }
    //}

    //void Update_Attack()
    //{
    //    view.Anim_Attack();
    //}

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
        view.Play_Clip_takeDamage();
    }
}