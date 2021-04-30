using System;
using UnityEngine;

public interface IGridEntity {

    event Action<IGridEntity> OnMove;
    event Action<IGridEntity> OnRemove;

    Vector3 Position { get; set; }
        
}