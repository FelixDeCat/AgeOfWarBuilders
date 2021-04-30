using System;
using UnityEngine;

namespace IA2.Clase5
{
    public class Enemy : MonoBehaviour, IGridEntity
    {

        public event Action<IGridEntity> OnMove;
        public event Action<IGridEntity> OnRemove;

        public int hp;
        public int damage;
        public EnemyType type;
        public LootType[] loot;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
    }

    public enum EnemyType
    {
        Archer,
        Melee,
        Mage
    }

    public enum LootType
    {
        Gold,
        HealthPotion,
        ManaPotion,
        Shield,
        Sword
    }
}