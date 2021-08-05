﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using c = VillagerCond;
using v = WorldValues;
using TMPro;

public class Villager : LivingEntity
{
    VillagerProfesion profession;
    float timer_recalculate_work_station;

    [Header("States")]
    [SerializeField] IdleState idleState;
    [SerializeField] RestState reststate;
    [SerializeField] WorkState workState;
    [SerializeField] HideState hideRestState;
    [SerializeField] HideState hideHealState;
    [SerializeField] EatState eatState;
    [SerializeField] FindToolState findToolState;
    [SerializeField] FindWeaponState findWeaponState;
    [SerializeField] CombatState combatState;
    [SerializeField] FindFood findFood;
    [SerializeField] HealState healstate;
    FiniteStateMachine myFsm;
    GOAPState currentState;
    [SerializeField] internal GOAPVillagerValueConditions villager_values_conditions;

    [Header("Components")]
    public GenericInteractor interactor;
    [SerializeField] GenericEnergyComponent Energy;
    [SerializeField] GenericEnergyComponent Hungry;
    [SerializeField] public VillagerInventory inventory;
    [SerializeField] ResourceHarvester harvester;
    [SerializeField] PlaceFinder placefinder;
    [SerializeField] PlaceFinderCombat placefinderCombat;
    public GridComponent MyGridComponentEntity;
    public Villager_View view;
    Threat mythreat;

    public bool HasWork = true;
    public bool inDanger = false;

    [Header("Debugs")]
    [SerializeField] string villagerName = "villager";
    public string VillagerName { get => villagerName; set { villagerName = value; villager_text_name.text = value; } }
    [SerializeField] TextMeshProUGUI villager_text_name;
    [SerializeField] TextMeshProUGUI debug_state;

    #region Init / DeInit / Tick

    protected override void OnInitialize()
    {
        base.OnInitialize();

        idleState.SetVillager(this);
        reststate.SetVillager(this);
        workState.SetVillager(this);
        hideRestState.SetVillager(this);
        hideHealState.SetVillager(this);
        eatState.SetVillager(this);
        findToolState.SetVillager(this);
        findWeaponState.SetVillager(this);
        combatState.SetVillager(this);
        findFood.SetVillager(this);
        healstate.SetVillager(this);

        idleState.OnNeedsReplan += Replan;
        reststate.OnNeedsReplan += Replan;
        workState.OnNeedsReplan += Replan;
        hideRestState.OnNeedsReplan += Replan;
        hideHealState.OnNeedsReplan += Replan;
        eatState.OnNeedsReplan += Replan;
        findToolState.OnNeedsReplan += Replan;
        findWeaponState.OnNeedsReplan += Replan;
        combatState.OnNeedsReplan += Replan;
        findFood.OnNeedsReplan += Replan;
        healstate.OnNeedsReplan += Replan;

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
        mythreat.Initialize();
        mythreat.Rise();

        Hungry.SpendAllEnergy();

        Invoke("Replan", 0.1f);

        Invoke("Replan", 2f);
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
        if (Input.GetKeyDown(KeyCode.Y)) inDanger = !inDanger;
        if (Input.GetKeyDown(KeyCode.U)) HasWork = !HasWork;

        if (Input.GetKeyDown(KeyCode.K))
        {
            GoToWork();
        }

        MyGridComponentEntity.Grid_RefreshComponent();
        mythreat.Tick(DeltaTime);
    }

    #endregion

    public void Go_To_Rest()
    {
        placefinder.ReconfigureExecution(() => { Energy.AddEnergy(5); Replan(); });
        placefinder.ReconfigurePredicate(x => x.action_type == ActionPlaces.rest);
        placefinder.Go_To_Execute_Action();
    }
    public void Go_To_Take_Weapon()
    {
        placefinder.ReconfigureExecution(() => { inventory.PickUpWeapon(); Replan(); });
        placefinder.ReconfigurePredicate(x => x.action_type == ActionPlaces.weapon);
        placefinder.Go_To_Execute_Action();
    }
    public void Go_To_Take_Tool()
    {
        placefinder.ReconfigureExecution(() => { inventory.PickUpTool(); Replan(); });
        placefinder.ReconfigurePredicate(x => x.action_type == ActionPlaces.tool);
        placefinder.Go_To_Execute_Action();
    }
    public void Go_To_Take_Food()
    {
        placefinder.ReconfigureExecution(() => { inventory.PickUpFood(); Replan(); });
        placefinder.ReconfigurePredicate(x => x.action_type == ActionPlaces.food);
        placefinder.Go_To_Execute_Action();
    }
    public void Go_To_Heal()
    {
        placefinder.ReconfigureExecution(() => { Heal(5); Replan(); });
        placefinder.ReconfigurePredicate(x => x.action_type == ActionPlaces.heal);
        placefinder.Go_To_Execute_Action();
    }

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
        var planner = new GoapPlanner();
        var plan = planner.Run(From(), To(), Actions(),StartCoroutine);
        DebugGOAP(plan);
    }

    #region Configure From-To States
    GOAPState From()
    {
        var from = new GOAPState();
        from.values[c.GAME_WIN] = false;
        from.values[c.I_AM_HUNGRY] = Hungry.Energy > villager_values_conditions.low_hungry_min;
        from.values[c.HAS_ENERGY] = Energy.Energy > villager_values_conditions.low_energy_min;
        from.values[c.HAS_LIFE] = HP > villager_values_conditions.low_life_min;
        from.values[c.HAS_WORK] = HasWork;
        from.values[c.IS_IN_DANGER] = inDanger;
        from.values[c.HAS_FOOD_IN_MY_INVENTORY] = inventory.HasFood;
        from.values[c.HAS_TOOL] = inventory.HasTool;
        from.values[c.HAS_WEAPON] = inventory.HasWeapon;
        return from;
    }
    GOAPState To()
    {
        var to = new GOAPState();
        to.values[c.GAME_WIN] = true;
        return to;
    }
    #endregion

    List<GOAPAction> Actions()
    {
        return new List<GOAPAction>
        {
            new GOAPAction(VillagerStatesNames.IDLE)
            .Pre(c.HAS_WORK, false)
            .Effect(c.HAS_WORK,     true)//¿los efectos no serían todos? ¿que filosofia tengo que tomar?
            .LinkedState(idleState),

            new GOAPAction(VillagerStatesNames.HEAL)
            .Pre(c.HAS_LIFE, false)
            .Pre(c.IS_IN_DANGER,    false)
            .Effect(c.HAS_WORK, true)
            .Effect(c.HAS_LIFE, true)
            //.Effect(c.HAS_TOOL, false)
            .Effect(c.HAS_WEAPON, false)
            .LinkedState(healstate),

            new GOAPAction(VillagerStatesNames.REST)
            .Pre(c.IS_IN_DANGER,    false)
            .Pre(c.HAS_ENERGY,      false)
            //.Effect(c.HAS_TOOL, false)
            .Effect(c.HAS_WORK, true)
            .Effect(c.HAS_WEAPON, false)
            .Effect(c.HAS_ENERGY,   true)
            .LinkedState(reststate),

            new GOAPAction(VillagerStatesNames.EAT)
            .Pre(c.HAS_FOOD_IN_MY_INVENTORY,    true)
            .Pre(c.I_AM_HUNGRY,                 true)
            .Effect(c.HAS_FOOD_IN_MY_INVENTORY, false)
            .Effect(c.I_AM_HUNGRY,              false)
          //  .Effect(c.HAS_TOOL, false)
         //  .Effect(c.HAS_WEAPON, false)
            .Effect(c.HAS_ENERGY, true)
            .LinkedState(eatState),

            new GOAPAction(VillagerStatesNames.HIDE_TO_HEAL)
            .Pre(c.IS_IN_DANGER,    true)
            .Pre(c.HAS_LIFE,        false)
            .Effect(c.HAS_LIFE,     true)
            .Effect(c.HAS_TOOL,     false)
            .Effect(c.HAS_WEAPON,   false)
            .LinkedState(hideHealState),

            new GOAPAction(VillagerStatesNames.HIDE_TO_REST)
            .Pre(c.IS_IN_DANGER,    true)
            .Pre(c.HAS_ENERGY,      false)
            .Effect(c.HAS_ENERGY,   true)
            .Effect(c.HAS_TOOL,     false)
            .Effect(c.HAS_WEAPON,   false)
            .LinkedState(hideRestState),

            new GOAPAction(VillagerStatesNames.WORK)
            .Pre(c.HAS_WORK,            true)
            .Pre(c.HAS_ENERGY,          true)
            .Pre(c.HAS_LIFE,            true)
            .Pre(c.I_AM_HUNGRY,         false)
            .Pre(c.HAS_TOOL,            true)
            .Pre(c.IS_IN_DANGER,        false)
            .Effect(c.HAS_WORK,         false)
            .Effect(c.GAME_WIN,         true)
            .Effect(c.HAS_ENERGY,       false)
            .Effect(c.I_AM_HUNGRY,      true)
            .LinkedState(workState),

            new GOAPAction(VillagerStatesNames.COMBAT)
            .Pre(c.IS_IN_DANGER,    true)
            .Pre(c.HAS_WEAPON,      true)
            .Pre(c.HAS_ENERGY,      true)
            .Pre(c.HAS_LIFE,        true)
            .Effect(c.IS_IN_DANGER, false)
            .Effect(c.HAS_ENERGY,   false)
            .Effect(c.HAS_LIFE,     false)
            .Effect(c.HAS_TOOL,     false)
            .Effect(c.I_AM_HUNGRY,  true)
            .LinkedState(combatState),

            new GOAPAction(VillagerStatesNames.FIND_FOOD)
            .Pre(c.I_AM_HUNGRY,                     true)
            .Pre(c.HAS_FOOD_IN_MY_INVENTORY,        false)
            .Effect(c.HAS_FOOD_IN_MY_INVENTORY,     true)
            .LinkedState(findFood),

            new GOAPAction(VillagerStatesNames.FIND_TOOL)
            .Pre(c.HAS_TOOL,        false)
            .Effect(c.HAS_TOOL,     true)
            .Effect(c.HAS_WEAPON,   false)
            .LinkedState(findToolState),

            new GOAPAction(VillagerStatesNames.FIND_WEAPON)
            .Pre(c.HAS_WEAPON,      false)
            .Pre(c.HAS_LIFE,        true)
            .Effect(c.HAS_WEAPON,   true)
            .Effect(c.HAS_TOOL,     false)
            .LinkedState(findWeaponState)

        };
    }

    #endregion

    #region SM
    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        if (myFsm != null) {
            myFsm.Active = false;
            //myFsm.Clear();
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
    public bool IAmVeryTired => Energy.Energy < villager_values_conditions.low_energy_min;

    ////////////////////////////////////////////////////
    //// HUNGRY
    ////////////////////////////////////////////////////
    public void RemoveHungry(int val) => Hungry.SpendEnergy(val);
    public void AddHungry(int val) => Hungry.AddEnergy(val);
    public bool MyHungryIsSatisfied => Hungry.EnergyEmpty();
    public bool VeryHungry() => Hungry.Energy > villager_values_conditions.low_hungry_min;

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
        //currentState.values[val] = !currentState.values[val];
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
    public const string HIDE_TO_REST = "Hide_To_Rest";
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
