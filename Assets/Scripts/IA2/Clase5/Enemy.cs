using System;
using UnityEngine;

namespace IA2.Clase5
{
    public class Enemy : MonoBehaviour, IGridEntity
    {

        public event Action<IGridEntity> OnMove;

        public int hp;
        public int damage;
        public EnemyType type;
        public LootType[] loot;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        Vector3 IGridEntity.Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IGridEntity.IsAlive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Grids gridType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private void Enemy_Pepe()
        {
            throw new NotImplementedException();
        }

        event Action Pepe;
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