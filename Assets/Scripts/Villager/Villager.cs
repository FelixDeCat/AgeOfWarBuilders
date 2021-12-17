using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using c = VillagerCond;
using v = WorldValues;
using TMPro;

public class Villager : LivingEntity
{
    #region variables_states
    [Header("States")]
    [SerializeField] WorkState workState;
    [SerializeField] HideState hideRestState;
    [SerializeField] FindToolState findToolState;
    [SerializeField] FindWeaponState findWeaponState;
    [SerializeField] CombatState combatState;
    #endregion
    #region variables_goap
    FiniteStateMachine myFsm;
    GOAPState currentState;
    GoapPlanner planner = new GoapPlanner();
    public bool HasWork = true;
    public bool inDanger = false;
    [SerializeField] internal GOAPVillagerValueConditions queryvalues;
    public const int EMPTY = 0; 
    public const int FILL = 100;
    #endregion
    #region variables_health
    [SerializeField] GenericEnergyComponent Energy;
    [SerializeField] GenericEnergyComponent Hungry;
    #endregion
    #region variables_work
    [SerializeField] VillagerProfesion profession;
    float timer_recalculate_work_station;
    public GenericInteractor interactor;
    [SerializeField] public VillagerInventory inventory;
    [SerializeField] ResourceHarvester harvester;
    [SerializeField] PlaceFinder placefinder;
    #endregion
    #region variables_combat
    [SerializeField] PlaceFinderCombat placefinderCombat;
    [SerializeField] NPC_CombatComponent combatComponent;
    #endregion
    #region variables_view
    public Villager_View view;
    #endregion
    #region variables_world_threat
    Threat mythreat;
    #endregion
    #region variables_grid
    public GridComponent MyGridComponentEntity;
    #endregion
    #region variables_steering
    public SmoothLookAt smoothLookAt;
    public FollowComponent follow_component;
    #endregion
    #region variables_debugging
    [Header("Debugs")]
    [SerializeField] string villagerName = "villager";
    public string VillagerName { get => villagerName; set { villagerName = value; villager_text_name.text = value; } }
    [SerializeField] TextMeshProUGUI villager_text_name;
    [SerializeField] TextMeshProUGUI debug_state;
    #endregion

    protected override void OnDeath()
    {
        base.OnDeath();
        Destroy(this.gameObject);
    }

    #region Init / DeInit / Tick
    protected override void OnInitialize()
    {
        base.OnInitialize();
        workState.SetVillager(this);
        hideRestState.SetVillager(this);
        findToolState.SetVillager(this);
        findWeaponState.SetVillager(this);
        combatState.SetVillager(this);
        workState.OnNeedsReplan += Replan;
        hideRestState.OnNeedsReplan += Replan;
        findToolState.OnNeedsReplan += Replan;
        findWeaponState.OnNeedsReplan += Replan;
        combatState.OnNeedsReplan += Replan;

        interactor.InitializeInteractor();
        interactor.ConfigurePredicate(x =>
        {
            if (GetComponent<VillagerSelector>()) return false;
            else return true;
        });

        harvester.Initialize();
        placefinder.Initialize();

        MyGridComponentEntity.Grid_Initialize(this.gameObject);
        MyGridComponentEntity.Grid_Rise();

        mythreat = GetComponentInChildren<Threat>();
        mythreat.Initialize(type);
        mythreat.Rise();

        combatComponent.Configure(this.transform, EnemyNear, NoEnemiesNear, x => GoToPositionWithPathFinder(x));

        Hungry.SpendAllEnergy();

        Invoke("Replan", 5f);

        combatComponent.BeginSensor();
    }
    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
        interactor.DeinitializeInteractor();
        harvester.Deinitialize();
        placefinder.Deinitialize();
        MyGridComponentEntity.Grid_Deinitialize();
        mythreat.Deinitialize();
    }
    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);

        if (Input.GetKeyDown(KeyCode.R)) Replan();
        if (Input.GetKeyDown(KeyCode.T))
        {
            ReceiveDamage(10);
            Replan();
        }
        // if (Input.GetKeyDown(KeyCode.L)) placefinder.FindPlace();
        if (Input.GetKeyDown(KeyCode.Y))
        {
            inDanger = !inDanger;
            Replan();
        }
        if (Input.GetKeyDown(KeyCode.U)) HasWork = !HasWork;

        if (Input.GetKeyDown(KeyCode.K))
        {
            GoToWork();
        }

        MyGridComponentEntity.Grid_RefreshComponent();
        mythreat.Tick(DeltaTime);
    }
    #endregion
    #region EnemySensor
    void EnemyNear()
    {
        inDanger = true;
        Replan();
    }
    void NoEnemiesNear()
    {
        inDanger = false;
        Replan();
        view.PLAY_ANIM_Walk();
    }
    #endregion
    #region Action Orders
    public void StopAction()
    {
        Debug.Log("Stop Action");
        placefinder.StopWork();
        placefinder.ReconfigureExecution(() => { });
        view.PLAY_ANIM_Walk();
    }
    public void Go_To_Combat()
    {
        harvester.StopWork();
        placefinder.StopWork();
        //combatComponent.
        Debug.Log("GOTOCOMBAT");
        combatComponent.GoToAttack();
    }
    public void Go_To_Rest()
    {
        placefinder.ReconfigureExecution(() => { Energy.AddEnergy(100); RemoveHungry(100); Replan(); inventory.DropWeaponAndTool(); Replan(); view.PLAY_ANIM_Walk(); });
        placefinder.ReconfigurePredicate(x => x.action_type == ActionPlaces.rest);
        placefinder.Go_To_Execute_Action();

    }
    public void Go_To_Take_Weapon()
    {
        view.PLAY_ANIM_Walk();
        placefinder.ReconfigureExecution(() => { inventory.PickUpWeapon(); Replan(); view.PLAY_ANIM_Walk(); });
        placefinder.ReconfigurePredicate(x => x.action_type == ActionPlaces.weapon);
        placefinder.Go_To_Execute_Action();
    }
    public void Go_To_Take_Tool()
    {
        view.PLAY_ANIM_Walk();
        placefinder.ReconfigureExecution(() => { inventory.PickUpTool(); Replan(); view.PLAY_ANIM_Walk(); });
        placefinder.ReconfigurePredicate(x => x.action_type == ActionPlaces.tool);
        placefinder.Go_To_Execute_Action();
    }
    public void Go_To_Take_Food()
    {
        view.PLAY_ANIM_Walk();
        placefinder.ReconfigureExecution(() => { inventory.PickUpFood(); Replan(); });
        placefinder.ReconfigurePredicate(x => x.action_type == ActionPlaces.food);
        placefinder.Go_To_Execute_Action();
    }
    public void Go_To_Heal()
    {
        view.PLAY_ANIM_Walk();
        placefinder.ReconfigureExecution(() => { Heal(5); Replan(); });
        placefinder.ReconfigurePredicate(x => x.action_type == ActionPlaces.heal);
        placefinder.Go_To_Execute_Action();
    }
    #endregion
    #region GOAP
    [System.Serializable]
    internal class GOAPVillagerValueConditions
    {
        [SerializeField] internal int low_life_min = 20;
        [SerializeField] internal int low_energy_min = 20;
        [SerializeField] internal int low_hungry_min = 20;
    }
    public void Replan()
    {
        Debug.Log("REPLANEANDO");
        GOAPState from = From();
        GOAPState to = To();
        List<GOAPAction> actions = new List<GOAPAction>();
        actions = Actions();
        var plan = planner.Run(from, to, actions, StartCoroutine, x => { /*Debug.Log(x);*/ });
        DebugGOAP(plan);
    }

    #region Configure From-To States
    GOAPState From()
    {
        var from = new GOAPState("FROM");
        
        //from.values[c.GAME_WIN] = false;
        //from.values[c.I_AM_HUNGRY] = Hungry.Energy > queryvalues.low_hungry_min;
        //from.values[c.HAS_ENERGY] = Energy.Energy > queryvalues.low_energy_min;
        //from.values[c.HAS_LIFE] = HP > queryvalues.low_life_min;
        //from.values[c.HAS_WORK] = HasWork;
        //from.values[c.IS_IN_DANGER] = inDanger;
        ////from.values[c.HAS_FOOD_IN_MY_INVENTORY] = inventory.HasFood;
        //from.values[c.HAS_TOOL] = inventory.HasTool;
        //from.values[c.HAS_WEAPON] = inventory.HasWeapon;

        from.currentState.Gamewin = false;
        from.currentState.Hungry = Hungry.Energy;
        from.currentState.Energy = Energy.Energy;
        from.currentState.Life = HP;
        from.currentState.HasWork = HasWork;
        from.currentState.IsInDanger = inDanger;
        from.currentState.Hastool = inventory.HasTool;
        from.currentState.HasWeapon = inventory.HasWeapon;


        return from;
    }
    GOAPState To()
    {
        var to = new GOAPState("TO");
        to.currentState.Gamewin = true;
        //to.values[c.GAME_WIN] = true;
        return to;
    }
    #endregion

    List<GOAPAction> Actions()
    {
        return new List<GOAPAction>
        {
            #region HIDE

            new GOAPAction(VillagerStatesNames.HIDE)

            .PreW(c.HAS_ENERGY,         x => x.Energy < queryvalues.low_energy_min)
            .EffectW(c.HAS_ENERGY,      x => x.Energy = FILL)
            .EffectW(c.I_AM_HUNGRY,     x => x.Hungry = EMPTY)

            .LinkedState(hideRestState),
            #endregion
            #region WORK
            new GOAPAction(VillagerStatesNames.WORK)

            //.Pre(c.HAS_WORK,            true)
            //.Pre(c.HAS_ENERGY,          true)
            //.Pre(c.HAS_LIFE,            true)
            //.Pre(c.HAS_TOOL,            true)
            //.Pre(c.IS_IN_DANGER,        false)
            //.Effect(c.HAS_WORK,         false)
            //.Effect(c.GAME_WIN,         true)
            //.Effect(c.HAS_ENERGY,       false)
            //.Effect(c.I_AM_HUNGRY,      true)

            .PreW(c.HAS_WORK,       x => x.HasWork)
            .PreW(c.HAS_ENERGY,     x => x.Energy > queryvalues.low_energy_min)
            .PreW(c.HAS_LIFE,       x => x.Life > queryvalues.low_life_min)
            .PreW(c.HAS_TOOL,       x => x.Hastool)
            .PreW(c.IS_IN_DANGER,   x => !x.IsInDanger)
            .EffectW(c.HAS_WORK,    x => x.HasWork = false)
            .EffectW(c.GAME_WIN,    x => x.Gamewin = true)
            .EffectW(c.HAS_ENERGY,  x => x.Energy = EMPTY)
            .EffectW(c.I_AM_HUNGRY, x => x.Hungry = FILL)

            .LinkedState(workState),
            #endregion
            #region COMBAT
            new GOAPAction(VillagerStatesNames.COMBAT)

            //.Pre(c.IS_IN_DANGER,    true)
            //.Pre(c.HAS_WEAPON,      true)
            //.Pre(c.HAS_ENERGY,      true)
            //.Pre(c.HAS_LIFE,        true)
            //.Effect(c.IS_IN_DANGER, false)
            //.Effect(c.HAS_ENERGY,   false)
            //.Effect(c.HAS_LIFE,     false)
            //.Effect(c.HAS_TOOL,     false)
            //.Effect(c.GAME_WIN,     true)
            //.Effect(c.I_AM_HUNGRY,  true)

            .PreW(c.IS_IN_DANGER,   x => x.IsInDanger)
            .PreW(c.HAS_WEAPON,     x => x.HasWeapon)
            .PreW(c.HAS_ENERGY,     x => x.Energy > queryvalues.low_energy_min)
            .PreW(c.HAS_LIFE,       x => x.Life > queryvalues.low_life_min)
            .EffectW(c.IS_IN_DANGER, x => x.IsInDanger = false)
            .EffectW(c.HAS_ENERGY,   x => x.Energy = EMPTY)
            .EffectW(c.HAS_LIFE,     x => x.Life = 1)
            .EffectW(c.HAS_TOOL,     x => x.Hastool = false)
            .EffectW(c.GAME_WIN,     x => x.Gamewin = true)
            .EffectW(c.I_AM_HUNGRY,  x => x.Hungry = FILL)

            .LinkedState(combatState),
            #endregion
            #region FIND TOOL
            new GOAPAction(VillagerStatesNames.FIND_TOOL)

            //.Pre(c.HAS_TOOL,        false)
            

            //.Effect(c.HAS_TOOL,     true)
            //.Effect(c.HAS_WEAPON,   false)

            .PreW(c.HAS_TOOL,       x => !x.Hastool)
            .EffectW(c.HAS_TOOL,    x => x.Hastool = true)
            .EffectW(c.HAS_WEAPON,  x => x.HasWeapon = false)

            .LinkedState(findToolState),
            #endregion
            #region FIND WEAPON
            new GOAPAction(VillagerStatesNames.FIND_WEAPON)

            //.Pre(c.HAS_WEAPON,      false)
            //.Pre(c.HAS_LIFE,        true)
            //.Pre(c.IS_IN_DANGER,    true)

            

            //.Effect(c.HAS_WEAPON,   true)
            //.Effect(c.HAS_TOOL,     false)

            .PreW(c.HAS_WEAPON,      x => !x.HasWeapon)
            .PreW(c.HAS_LIFE,        x => x.Life > queryvalues.low_life_min)
            .PreW(c.IS_IN_DANGER,    x => x.IsInDanger)
            .EffectW(c.HAS_WEAPON,   x => x.HasWeapon = true)
            .EffectW(c.HAS_TOOL,     x => x.Hastool = false)


            .LinkedState(findWeaponState)
            #endregion
        };
    }

    #endregion
    #region SM
    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        if (myFsm != null)
        {
            myFsm.Active = false;
            myFsm.Clear();
        }
        myFsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        myFsm.Active = true;
    }
    #endregion
    #region Energy and Hungry

    ////////////////////////////////////////////////////
    //// ENERGY
    ////////////////////////////////////////////////////
    public void AddEnergy(int val) => Energy.AddEnergy(val);
    public void SpendEnergy(int val) => Energy.SpendEnergy(val);
    public bool EnergyIsFull => Energy.EnergyIsFull();
    public bool IAmVeryTired => Energy.Energy < queryvalues.low_energy_min;

    ////////////////////////////////////////////////////
    //// HUNGRY
    ////////////////////////////////////////////////////
    public void RemoveHungry(int val) => Hungry.SpendEnergy(val);
    public void AddHungry(int val) => Hungry.AddEnergy(val);
    public bool MyHungryIsSatisfied => Hungry.EnergyEmpty();
    public bool VeryHungry() => Hungry.Energy > queryvalues.low_hungry_min;

    #endregion
    #region Profession
    public void ConfigureProfession(VillagerProfesion _profesion)
    {
        profession = _profesion;
        harvester.ReconfigureProfession();
        Invoke("GoToWork", 0.1f);
    }

    public VillagerProfesion GetProfession() => profession;

    public void GoToWork()
    {
        harvester.BeginWork();
    }
    public void StopWork()
    {
        harvester.StopWork();
    }
    #endregion
    #region Axiliars DEBUGs
    void DebugGOAP(IEnumerable<GOAPAction> plan)
    {
        GoapDebug.RefreshState(From());
        if (plan == null) GoapDebug.Message("El path no contiene elementos");
        else { GoapDebug.RefresPlan(plan); ConfigureFsm(plan); }
    }
    public void DebugState(string value) => debug_state.text = value;
    /// <summary>
    /// le paso un valor, para switchearlo en el caso de usar el current para debugear el plan
    /// </summary>
    /// <param name="val"></param>
    public void DebugChangeAState(string val)
    {
        Replan();
    }
    #endregion
}

public static class VillagerStatesNames
{
    public const string IDLE = "Idle";
    public const string HEAL = "Heal";
    public const string REST = "Rest";
    public const string EAT = "Eat";
    public const string HIDE_TO_HEAL = "Hide_To_Heal";
    public const string HIDE = "Hide_To_Rest";
    public const string WORK = "Work";
    public const string COMBAT = "Combat";
    public const string FIND_FOOD = "FindFood";
    public const string FIND_TOOL = "FindTool";
    public const string FIND_WEAPON = "FindWeapon";
}
public static class WorldValues
{
    public const string ENERGY = "Energy";
}
public static class VillagerCond
{
    public const string GAME_WIN = "GameWin";
    public const string I_AM_HUNGRY = "I_am_Hungry";
    public const string HAS_ENERGY = "Has_Energy";
    public const string HAS_LIFE = "Has_Life";
    public const string HAS_WORK = "Has_Work";
    public const string HAS_FOOD_IN_MY_INVENTORY = "HasFoodInInventory";
    public const string IS_IN_DANGER = "Is_In_Danger";
    public const string HAS_TOOL = "Has_Tool";
    public const string HAS_WEAPON = "Has_Weapon";
}
public enum VillagerProfesion
{
    miner, farmer, lumberjack, warrior
}
