using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RefreshSubscriber : MonoBehaviour
{
    List<Action> collection_to_refresh = new List<Action>();

    public void SubscribeToRefresh(Action function_to_refresh)
    {
        collection_to_refresh.Add(function_to_refresh);
    }

    private void Update()
    {
        foreach (var c in collection_to_refresh)
        {
            c.Invoke();
        }

    }
}
