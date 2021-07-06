using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    public Dictionary<ResourceType, int> resources_store = new Dictionary<ResourceType, int>();
    Dictionary<ResourceType, ResourceData> resources_modeldatabase = new Dictionary<ResourceType, ResourceData>();
    public ResourceData[] data;

    [SerializeField] UI_Resources ui;

    public int maxCapacity;

    ResourceType res;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeResources();
    }

    void InitializeResources()
    {
        for (int i = 0; i < data.Length; i++)
        {
            resources_store.Add(data[i].Type, 0);
            resources_modeldatabase.Add(data[i].Type, data[i]);
            ui.CreateUI(data[i].GetSprite(),"0", data[i].Type);
        }
    }

    public static bool AddResource(int quantity, ResourceType type) => instance.AddRes(quantity, type);

    public bool AddRes(int quantity, ResourceType type)
    {
        var current = resources_store[type];
        if (current == maxCapacity)
        {
            ui.AnimateCanNotAddResource(type);
            return false;
        }
        else
        {
            current += quantity;
            if (current > maxCapacity)
            {
                current = maxCapacity;
            }
            resources_store[type] = current;
            ui.UpdateResource(type, current.ToString());
            ui.AnimateAddResource(type);
            return true;
        }
    }

    public bool RemoveResource(int quantity, ResourceType type)
    {
        var current = resources_store[type];

        if (current < quantity)
        {
            ui.AnimateCanNotRemoveResource(type);
            return false;
        }
        else
        {
            current -= quantity;
            if (current < 0) current = 0;
            resources_store[type] = current;
            ui.UpdateResource(type, current.ToString());
            ui.AnimateRemoveResource(type);
            return true;
        }
    }
}
