using Core.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Interface
{
    public interface IDamageable
    {
        int Health { get; }

        /// <summary>
        /// Event that is invoked when initial health set.
        /// </summary>
        CustomEvent<int> OnHealthSet { get; }

        /// <summary>
        /// Event that is invoked when current health is changed.
        /// </summary>
        CustomEvent<int> OnHealthChanged { get; }

        /// <summary>
        /// Event that is invoked when health is zeroed out.
        /// </summary>
        CustomEvent OnHealthZeroed { get; }

        void SetInitialHealth(int health);

        /// <summary>
        /// Decreases health by given damage.
        /// </summary>
        /// <param name="damage"></param>
        void TakeDamage(int damage = 1);
    }
}