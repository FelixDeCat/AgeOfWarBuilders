using System;
using UnityEngine;

public interface IGridEntity {

    event Action<IGridEntity> OnMove;

    Vector3 Position { get; set; }
        
}