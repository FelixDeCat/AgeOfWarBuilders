using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ResourceHarvester : MonoBehaviour
{
    float timer;
    float timer_recalculate;
    [SerializeField] float cd_harvest = 1f;
    [SerializeField] float cd_recalculate = 0.5f;
    bool in_harvest;

    bool active;

    [SerializeField] Villager villager;
    [SerializeField] GenericInteractor interactor;
    [SerializeField] CircleQuery QuerieResourceFinder;

    Resource current_resource;

    string MyProfessionName => villager.GetProfession().ToString();
    VillagerProfesion MyProfessionType => villager.GetProfession();

    bool canExplodeResource;

    public void ReconfigureProfession()
    {
        //le digo al interactor que me filtre y me guarde los resources del tipo que quiero

        interactor.AddFilter(MyProfessionName, x =>
        {
            var r = x.GetComponent<Resource>();
            if (r != null)
            {
                switch (MyProfessionType)
                {
                    case VillagerProfesion.miner: return r.GetResourceType == ResourceType.metal;
                    case VillagerProfesion.farmer: return r.GetResourceType == ResourceType.food;
                    case VillagerProfesion.lumberjack: return r.GetResourceType == ResourceType.wood;
                    case VillagerProfesion.warrior: return r.GetResourceType == ResourceType.combat_pos;
                    default: return false;
                }
            }
            else
            {
                return false;
            }
        });
    }

    public Resource GetResourceToWork()
    {

        return QuerieResourceFinder.Query()
                .OfType<GridComponent>()
                .Where(x => x.Grid_Object.GetComponent<Resource>())
                .Select(x => x.Grid_Object.GetComponent<Resource>())
                .Where(x => x.GetResourceType == GetResByProfession(MyProfessionType))
                .OrderBy(x => Vector3.Distance(x.transform.position, this.transform.position))
                .First();
    }

    public void BeginWork()
    {
        current_resource = null;
        canExplodeResource = true;
        current_resource = GetResourceToWork();
        // Move_to(resource.pos_to_work, IHaveArrived);
        villager.GoToPositionWithPathFinder(current_resource.pos_to_work.position);
        villager.Callback_IHaveArrived(IHaveArrived);
    }

    public void StopWork()
    {
        canExplodeResource = false;
        current_resource = null;
        EndHarvest();
    }

    void IHaveArrived()
    {
        if (Vector3.Distance(current_resource.pos_to_work.position, villager.transform.position) < 0.5f)
        {
            BeginHarvest();
        }
        else
        {
            villager.view.PLAY_ANIM_Idle();
            villager.LerpPosRot(current_resource.pos_to_work, EndLerp);
        }
    }
    void EndLerp()
    {
        BeginHarvest();
    }

    void OnInteractableAdded(GenericInteractable i)
    {

    }
    void OnInteractableRemoved(GenericInteractable i)
    {

    }

    public void Initialize()
    {
        active = true;
        ReconfigureProfession();
        interactor.ADD_CALLBACK_Add_Interactable(OnInteractableAdded);
        interactor.ADD_CALLBACK_Remove_Interactable(OnInteractableRemoved);
        QuerieResourceFinder.Configure(villager.transform);
    }
    public void Deinitialize()
    {
        canExplodeResource = false;
        active = false;
        timer = 0;
        timer_recalculate = 0;
        in_harvest = false;
    }

    public void BeginHarvest()
    {
        if (in_harvest) return;
        in_harvest = true;
        timer = 0;
        active = true;
    }
    public void EndHarvest()
    {
        in_harvest = false;
        timer = 0;
        active = false;
        villager.view.PLAY_ANIM_Walk();
    }

    ResourceType GetResByProfession(VillagerProfesion profesion)
    {
        switch (profesion)
        {
            case VillagerProfesion.miner: return ResourceType.metal;
            case VillagerProfesion.farmer: return ResourceType.food;
            case VillagerProfesion.lumberjack: return ResourceType.wood;
            case VillagerProfesion.warrior: return ResourceType.combat_pos;
            default: return ResourceType.none;
        }
    }

    private void Update()
    {
        if (!active) return;

        if (in_harvest)
        {
            if (timer < cd_harvest)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {

                var resource = interactor
                    .GetFilteredCollection(villager.GetProfession().ToString())
                    .Select(x => x.GetComponent<Resource>())
                    .FirstOrDefault();

                if (resource == null) { return; }

                var package = resource.GetElement();

                villager.AddHungry(10);
                villager.SpendEnergy(10);

                if (package == null)
                {
                    in_harvest = false;
                    timer = 0;
                }
                else
                {
                    switch (resource.GetResourceType)
                    {
                        case ResourceType.none: villager.view.PLAY_ANIM_Idle(); break;
                        case ResourceType.wood: villager.view.PLAY_ANIM_Hit(); break;
                        case ResourceType.food: villager.view.PLAY_ANIM_Farm(); break;
                        case ResourceType.metal: villager.view.PLAY_ANIM_Hit(); break;
                        case ResourceType.combat_pos: villager.view.PLAY_ANIM_Shoot(); break;
                    }

                    if (resource.GetResourceType != ResourceType.combat_pos)
                    {
                        ResourceManager.AddResource(package.Quant, package.ResType);
                    }
                    else
                    {

                    }
                    timer = 0;
                }

                villager.Replan();
            }
        }
    }
}
