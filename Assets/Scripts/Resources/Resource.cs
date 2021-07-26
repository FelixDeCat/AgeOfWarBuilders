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
            var pack = new ResourcePackage(1, type);
            Check();
            return pack;
        }
        else
        {
            if (isRenovable)
            {
                //feedback de "Esperá a que se cargue"
            }
            else
            {
                //feedback de "Esto ya no se va a cargar mas"
            }
            return null;
        }
    }

    void Check()
    {
        if (capacity <= 0)
        {
            //feedback unico de "¡Pum! justo se te terminó"
        }
        else
        {
            //feedback de "entrego el paquete"
        }
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
