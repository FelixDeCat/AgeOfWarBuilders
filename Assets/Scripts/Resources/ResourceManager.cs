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

    public event Action OnChange;

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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddRes(5, ResourceType.wood);
        }
    }

    public void ForceEventChange()
    {
        OnChange?.Invoke();
    }

    public void SubscribeToChanges(Action _onChange)
    {
        OnChange += _onChange;
    }

    public Sprite GetSprite(ResourceType val)
    {
        return resources_modeldatabase[val].GetSprite();
    }

    public bool SpendResourcePackage(Tuple<ResourceType, int>[] to_spend)
    {
        if (to_spend.Length <= 0 || resources_store.Count <= 0) return false;
        for (int i = 0; i < to_spend.Length; i++)
        {
            if (!IHaveElement(to_spend[i].Item1, to_spend[i].Item2)) return false;
            else continue;
        }

        for (int i = 0; i < to_spend.Length; i++)
        {
            RawSubstractElement(to_spend[i].Item1, to_spend[i].Item2);
        }

        OnChange?.Invoke();

        return true;
    }

    public bool IHaveThisResourcePackage(Tuple<ResourceType, int>[] to_check)
    {
        if (to_check.Length <= 0 || resources_store.Count <= 0) return false;
        for (int i = 0; i < to_check.Length; i++)
        {
            if (!IHaveElement(to_check[i].Item1, to_check[i].Item2)) return false;
            else continue;
        }
        return true;
    }

    bool IHaveElement(ResourceType type, int quantity)
    {
        if (!resources_store.ContainsKey(type)) return false;
        if (resources_store[type] < quantity) return false;
        else return true;
    }

    void RawSubstractElement(ResourceType type, int quantity)
    {
        resources_store[type] -= quantity;
        if (resources_store[type] < 0) resources_store[type] = 0;

        ui.UpdateResource(type, resources_store[type].ToString());
        ui.AnimateRemoveResource(type);

        OnChange?.Invoke();
    }
    bool SubstractElement(ResourceType type, int quantity)
    {
        if (!resources_store.ContainsKey(type)) return false;
        if (resources_store[type] >= quantity)
        {
            resources_store[type] -= quantity;
            if (resources_store[type] < 0) resources_store[type] = 0;
            OnChange?.Invoke();
            return true;
        }
        else {
            OnChange?.Invoke();
            return false;
        }
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
            OnChange?.Invoke();
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
            OnChange?.Invoke();
            return true;
        }
    }

    public bool RemoveResource(int quantity, ResourceType type)
    {
        var current = resources_store[type];

        if (current < quantity)
        {
            ui.AnimateCanNotRemoveResource(type);
            OnChange?.Invoke();
            return false;
        }
        else
        {
            current -= quantity;
            if (current < 0) current = 0;
            resources_store[type] = current;
            ui.UpdateResource(type, current.ToString());
            ui.AnimateRemoveResource(type);
            OnChange?.Invoke();
            return true;
        }
    }
}
