using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] int capacity = 10;
    public int Capacity => capacity;
    [SerializeField] bool isRenovable;
    public bool IsRenovable => isRenovable;
    [SerializeField] ResourceType type;
    public ResourceType GetResourceType => type;

    [SerializeField] GridComponent gridComponent;

    public Transform pos_to_work;

    public bool CanHarvest => capacity > 0;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        gridComponent.Grid_Initialize(this.gameObject);
        gridComponent.Grid_Rise();
    }
    public void Deinitialize()
    {
        gridComponent.Grid_Deinitialize();
        gridComponent.Grid_Death();
    }

    public ResourcePackage GetElement()
    {
        if (capacity > 0)
        {
            capacity--;
            if (!isRenovable)
            {
                if (capacity <= 0)
                {
                    Destroy();
                }
            }

            return new ResourcePackage(1, type);
        }
        else
        {
            if (!isRenovable)
            {
                //recurso agotado
                //no renovable
            }
            else
            {
                //recurso agotado
                //espera a que se renueve
            }

            return null;
        }
    }

    public void Destroy()
    {

    }
}
public enum ResourceType
{
    none,
    wood,
    food,
    metal,
    combat_pos
}
public class ResourcePackage
{
    int cant = -1;
    ResourceType type = ResourceType.none;

    public ResourcePackage()
    {
        cant = -1;
        type = ResourceType.none;
    }

    public ResourcePackage(int cant, ResourceType type)
    {
        this.cant = cant;
        this.type = type;
    }

    public int Quant => cant;
    public ResourceType ResType => type;
}
