using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FindNodesAndAdd : MonoBehaviour
{
    public bool add;
    public bool execute;
    
    public LocalNodeHandler manager;
    public bool clamp_to_floor;
    public bool rename;
    public float dist_to_eliminate = 0.1f;

    public bool switch_UseGrids;

    [Header("Gizmos")]
    public bool switch_render;
    public bool draw_neighbors;
    public bool draw_radius;

    

    [Header("Debug")]
    public bool is_render_gizmos = false;
    public bool is_using_grids = true;

    float timer;

    public void OnEnable()
    {
        execute = false;
    }

    private void Update()
    {
        if (!execute) return;

        

        if (rename)
        {
            rename = false;
            var nodes = GetComponentsInChildren<IA_Felix.Node>();
            for (int i = 0; i < nodes.Length; i++) nodes[i].gameObject.name = "Node ["+ String.Format("{0:###}", i)+ "] weight:" + nodes[i].costs.external_weight;
        }

        manager.Find();

        if (switch_UseGrids)
        {
            switch_UseGrids = false;
            is_using_grids = !is_using_grids;
            manager.ChangeUseGrid(is_using_grids);
        }

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

        
        
        if (switch_render)
        {
            switch_render = false;
            is_render_gizmos = !is_render_gizmos;
            manager.Render(is_render_gizmos, draw_neighbors, draw_radius);
        }


        

    }
}
