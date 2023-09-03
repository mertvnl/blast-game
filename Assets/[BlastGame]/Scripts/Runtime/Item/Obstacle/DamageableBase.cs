using BlastGame.Interface;
using Core.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public abstract class DamageableBase : MonoBehaviour, IDamageable
    {
        public int Health { get; private set; }
        public CustomEvent<int> OnHealthSet { get; private set; } = new();
        public CustomEvent<int> OnHealthChanged { get; private set; } = new();
        public CustomEvent OnHealthZeroed { get; private set; } = new();

        public bool CanTakeDamage => Health > 0;

        public void SetInitialHealth(int health)
        {
            Health = health;
            OnHealthSet.Invoke(Health);
        }

        public void TakeDamage(int damage = 1)
        {
            if (!CanTakeDamage)
                return;

            Health -= damage;
            OnHealthChanged.Invoke(Health);
            CheckHealth();
        }

        private void CheckHealth()
        {
            if (Health <= 0)
                OnHealthZeroed.Invoke();
        }
    }
}