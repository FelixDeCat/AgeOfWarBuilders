using System;
using UnityEngine;

public interface IGridEntity {

    event Action<IGridEntity> OnMove;
    Grids gridType { get; }
    Vector3 Position { get; set; }
    bool IsAlive { get; set; }
        
}