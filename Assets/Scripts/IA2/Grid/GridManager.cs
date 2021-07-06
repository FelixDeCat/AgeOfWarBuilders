using UnityEngine;
using System.Collections.Generic;
using System;
public enum Grids { entity, thread, node, resource }
public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    Dictionary<Grids, SpatialGrid> spatialGrids = new Dictionary<Grids, SpatialGrid>();
    private void Awake()
    {
        instance = this;
        spatialGrids.Add(Grids.entity, entity_Grid);
        spatialGrids.Add(Grids.thread, thread_Grid);
        spatialGrids.Add(Grids.node, node_Grid);
        spatialGrids.Add(Grids.resource, resource_Grid);
    }
    [SerializeField] SpatialGrid entity_Grid;
    [SerializeField] SpatialGrid thread_Grid;
    [SerializeField] SpatialGrid node_Grid;
    [SerializeField] SpatialGrid resource_Grid;
    public static SpatialGrid EntityGrid            => instance.entity_Grid;
    public static SpatialGrid ThreadGrid            => instance.thread_Grid;
    public static SpatialGrid NodeGrid              => instance.node_Grid;
    public static SpatialGrid Resources             => instance.resource_Grid;
    public static SpatialGrid GetGrid(Grids grid)   => instance.spatialGrids[grid];
}
