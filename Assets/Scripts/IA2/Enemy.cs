using System;
using UnityEngine;

public class Enemy : WalkingEntity, IGridEntity
{
    public event Action<IGridEntity> OnMove;

    public int hp;
    public int damage;
    
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}