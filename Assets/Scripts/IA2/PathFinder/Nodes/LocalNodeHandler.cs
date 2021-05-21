using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IA_Felix;

public class LocalNodeHandler : MonoBehaviour
{
    List<Node> nodes;

    public static LocalNodeHandler instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Invoke("Find", 0.1f);
    }

    public void Find()
    {
        nodes = this.transform.GetComponentsInChildren<Node>().ToList();
        nodes.ForEach(x => x.OnStart(Refresh));
        Invoke("Refresh", 0.1f);
    }

    public void Refresh()
    {
        nodes.ForEach(x => x.Execute());
        nodes.ForEach(x => x.RefreshReConnections());
    }

    public void Render(bool isdraw, bool draw_neighbors, bool draw_Radius)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].render.draw_gizmos = isdraw;
            nodes[i].render.draw_neighbors = draw_neighbors;
            nodes[i].render.draw_radius = draw_Radius;
        }
    }
    public void ChangeUseGrid(bool is_using_grids)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].OnEditMode = !is_using_grids;
        }
    }
}
