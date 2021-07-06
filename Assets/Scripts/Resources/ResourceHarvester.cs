using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public void ReconfigureProfession()
    {
        var prof = villager.GetProfession();

        //le digo al interactor que me filtre y me guarde los resources del tipo que quiero
        interactor.AddFilter(prof.ToString(), x =>
        {
            var r = x.GetComponent<Resource>();
            if (r != null)
            {
                switch (prof)
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

    public void FindResource()
    {
        var col = interactor.GetFilteredCollection(villager.GetProfession().ToString());
        if (col.Count <= 0)
        {
            var resource = QuerieResourceFinder.Query()
                .OfType<GridComponent>()
                .Where(x => x.Grid_Object.GetComponent<Resource>())
                .Select(x => x.Grid_Object.GetComponent<Resource>())
                .Where(x => x.GetResourceType == GetResByProfession(villager.GetProfession()))
                .OrderBy(x => Vector3.Distance(x.transform.position, this.transform.position))
                .FirstOrDefault();

            Debug.Log("El resource que encontre es::: " + resource.gameObject.name);

            if (resource)
            {
                villager.transform.position = resource.pos_to_work.position;
                villager.transform.eulerAngles = resource.pos_to_work.eulerAngles;
                BeginHarvest();
            }
        }
        else
        {
            BeginHarvest();
        }
    }

    public void Initialize()
    {
        active = true;
        ReconfigureProfession();
        QuerieResourceFinder.Configure(villager.transform);
    }
    public void Deinitialize()
    {
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
    }
    public void EndHarvest()
    {
        in_harvest = false;
        timer = 0;
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

        if (timer_recalculate < cd_recalculate)
        {
            timer_recalculate = timer_recalculate + 1 * Time.deltaTime;
            var col = interactor.GetFilteredCollection(villager.GetProfession().ToString());
            Debug.Log("Col:>" + col.Count);
            if (col.Count > 0)
            {
                in_harvest = true;
            }
            else
            {
                in_harvest = false;
            }
        }
        else
        {
            timer_recalculate = 0;
        }

        if (in_harvest)
        {
            if (timer < cd_harvest)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {

                Debug.Log("Estoy recolectando");
                var resource = interactor
                    .GetFilteredCollection(villager.GetProfession().ToString())
                    .Select(x => x.GetComponent<Resource>())
                    .FirstOrDefault();


                var package = resource.GetElement();

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
            }
        }
    }
}
