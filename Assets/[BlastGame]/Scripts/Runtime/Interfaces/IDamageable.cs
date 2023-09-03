using Core.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Interface
{
    public interface IDamageable
    {
        int Health { get; }
        CustomEvent<int> OnHealthSet { get; }
        CustomEvent<int> OnHealthChanged { get; }
        CustomEvent OnHealthZeroed { get; }
        void SetInitialHealth(int health);
        void TakeDamage(int damage = 1);
    }
}