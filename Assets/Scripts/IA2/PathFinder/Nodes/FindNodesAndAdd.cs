using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FindNodesAndAdd : MonoBehaviour
{
    public bool add;
    public bool execute;
    public bool render;
    public LocalNodeHandler manager;
    public bool clamp_to_floor;
    public bool rename;
    public float dist_to_eliminate = 0.1f;

    bool canrender;

    float timer;

    public void OnEnable()
    {
        execute = false;
    }

    private void Update()
    {
        if (!execute) return;

        //if (timer < 0.2f)
        //{
        //    timer = timer + 1 * Time.deltaTime;
        //}
        //else
        //{
        //    Debug.Log("JJEJEJCUTANDO;");
        //    timer = 0;
        //    manager.Find();
        //}

        if (rename)
        {
            rename = false;
            var nodes = GetComponentsInChildren<IA_Felix.Node>();
            for (int i = 0; i < nodes.Length; i++) nodes[i].gameObject.name = "Node ["+ String.Format("{0:###}", i)+ "]";
        }


        manager.Find();

        if (clamp_to_floor)
        {
            clamp_to_floor = false;

            var nodes = GetComponentsInChildren<IA_Felix.Node>();

            foreach (var n in nodes)
            {
                n.ClampToFloor();
            }
        }


        if (add)
        {
            add = false;

            var nodes = GetComponentsInChildren<IA_Felix.Node>();

            foreach (var n in nodes)
            {
                n.transform.SetParent(this.transform);
            }
        }
        
        if (render)
        {
            render = false;
            canrender = !canrender;
            manager.Render(canrender);

        }
        
    }
}
