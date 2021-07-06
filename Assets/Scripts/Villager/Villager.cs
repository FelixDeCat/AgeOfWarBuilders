using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using c = VillagerCond;
using TMPro;

public enum VillagerProfesion
{
    miner, farmer, lumberjack, warrior
}
public class Villager : LivingEntity
{
    VillagerProfesion profession;

    public void ConfigureProfession(VillagerProfesion _profesion)
    {
        profession = _profesion;
        harvester.ReconfigureProfession();
    }
    public VillagerProfesion GetProfession() => profession;

    [SerializeField] string villagerName = "villager";
    public string VillagerName
    {
        get => villagerName;
        set
        {
            villagerName = value; villager_text_name.text = value;
        }
    }
    [SerializeField] TextMeshProUGUI villager_text_name;

    public GenericInteractor interactor;

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

    [SerializeField] TextMeshProUGUI debug_state;

    FiniteStateMachine myFsm;

    GOAPState currentState;

    [SerializeField] GOAPVillagerValueConditions villager_values_conditions;

    [SerializeField] GenericEnergyComponent Energy;
    [SerializeField] GenericEnergyComponent Hungry;

    [SerializeField] public VillagerInventory inventory;

    [SerializeField] ResourceHarvester harvester;
    [SerializeField] PlaceFinder placefinder;
    [SerializeField] PlaceFinderCombat placefinderCombat;

    public Villager_View view;

    public bool HasWork = true;
    public bool inDanger = false;


    ////////////////////////////////////////////////////
    //// ENERGY
    ////////////////////////////////////////////////////
    public void AddEnergy(int val)
    {
        Energy.AddEnergy(val);
    }
    public void SpendEnergy(int val)
    {
        Energy.SpendEnergy(val);
    }
    public bool EnergyIsFull => Energy.EnergyIsFull();

    ////////////////////////////////////////////////////
    //// HUNGRY
    ////////////////////////////////////////////////////
    public void RemoveHungry(int val)
    {
        Hungry.SpendEnergy(val);
    }
    public void AddHungry(int val)
    {
        Hungry.AddEnergy(val);
    }
    public bool MyHungryIsSatisfied => Hungry.EnergyEmpty();



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
        placefinderCombat.Initialize();

        Invoke("Replan", 0.1f);
    }
    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
        interactor.DeinitializeInteractor();
        harvester.Deinitialize();
        placefinder.Deinitialize();
        placefinderCombat.Deinitialize();
    }

    float timer_recalculate_work_station;
    protected override void OnTick(float DeltaTime)
    {
        base.OnTick(DeltaTime);

        if (timer_recalculate_work_station < 1f)
        {
            timer_recalculate_work_station = timer_recalculate_work_station + 1 * Time.deltaTime;
        }
        else
        {
            harvester.FindResource();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Replan();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ReceiveDamage(10);
            Replan();
        }

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    harvester.FindResource();
        //}

        if (Input.GetKeyDown(KeyCode.L))
        {
            placefinder.FindPlace();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            inDanger = !inDanger;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            HasWork = !HasWork;
        }


    }

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
        var plan = planner.Run(From(), To(), Actions(), StartCoroutine);

        GoapDebug.RefreshState(From());

        if (plan == null)
        {
            GoapDebug.Message("El path no contiene elementos");
        }
        else
        {
            GoapDebug.RefresPlan(plan);
            ConfigureFsm(plan);
        }
    }
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
            .Effect(c.HAS_LIFE, true)
            .Effect(c.HAS_TOOL, false)
            .Effect(c.HAS_WEAPON, false)
            .LinkedState(healstate),

            new GOAPAction(VillagerStatesNames.REST)
            .Pre(c.IS_IN_DANGER,    false)
            .Pre(c.HAS_ENERGY,      false)
            .Effect(c.HAS_TOOL, false)
            .Effect(c.HAS_WEAPON, false)
            .Effect(c.HAS_ENERGY,   true)
            .LinkedState(reststate),

            new GOAPAction(VillagerStatesNames.EAT)
            .Pre(c.HAS_FOOD_IN_MY_INVENTORY,    true)
            .Pre(c.I_AM_HUNGRY,                 true)
            .Effect(c.HAS_FOOD_IN_MY_INVENTORY, false)
            .Effect(c.I_AM_HUNGRY,              false)
            .Effect(c.HAS_TOOL, false)
            .Effect(c.HAS_WEAPON, false)
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
    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        Debug.Log("Completed Plan");
        myFsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        myFsm.Active = true;
    }

    public void DebugState(string value) => debug_state.text = value;

    public void DebugChangeAState(string val)
    {
        currentState.values[val] = !currentState.values[val];
        Replan();

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
