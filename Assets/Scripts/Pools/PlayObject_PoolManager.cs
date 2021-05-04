using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Pool;
using System;

public class PlayObject_PoolManager : MonoBehaviour
{

    public static PlayObject_PoolManager instance;
    private void Awake()
    {
        instance = this;
    }
    Dictionary<PlayObjectType, Transform> parents = new Dictionary<PlayObjectType, Transform>();
    Dictionary<PlayObjectType, Tuple<PlayObject, GenericPoolManager<PlayObject>>> poolDatabase = new Dictionary<PlayObjectType, Tuple<PlayObject, GenericPoolManager<PlayObject>>>();

    public void Feed(PlayObject newObject, Transform parent)
    {
        if (!poolDatabase.ContainsKey(newObject.type))
        {
            parents.Add(newObject.type, parent);
            poolDatabase.Add(newObject.type, Tuple.Create(newObject, new GenericPoolManager<PlayObject>(Create, x => x.gameObject.SetActive(true), x => x.gameObject.SetActive(false), newObject.type)));
        }
    }

    public PlayObject Get(PlayObjectType type, Vector3 position, Vector3 rotation_euler)
    {
        if (poolDatabase.ContainsKey(type))
        {
            var obj = poolDatabase[type].Item2.Get();
            obj.gameObject.SetActive(true);
            obj.gameObject.transform.position = position;
            obj.gameObject.transform.eulerAngles = rotation_euler;
            obj.On();
            return obj;
        }
        else
        {
            throw new Exception("No me Feedearon, No tengo Indexado un PoolManager de ese TYPE");
        }
    }

    public void Return(PlayObject obj)
    {
        var type = obj.type;

        if (poolDatabase.ContainsKey(type))
        {
            obj.Off();
            obj.gameObject.SetActive(false);
            poolDatabase[type].Item2.Return(obj);
        }
    }

    PlayObject Create(Func<object> indexerType)
    {
        var type = (PlayObjectType)indexerType.Invoke();

        if (poolDatabase.ContainsKey(type))
        {
            PlayObject go = GameObject.Instantiate(poolDatabase[type].Item1, this.transform.position, this.transform.rotation, parents[type]);
            go.gameObject.SetActive(false);
            return go;
        }
        else
        {
            throw new System.Exception("No contiene la Key");
        }
            
        
    }

}
