using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime.Models
{
    public abstract class ItemData : ScriptableObject
    {
        [field: SerializeField] public ItemBase ItemPrefab { get; private set; }
        [field: SerializeField] public ParticleSystem DisposeParticle { get; private set; }
        [field: SerializeField] public Sprite DefaultSprite { get; private set; }
        /// <summary>
        /// Score amount that item gives after a successful blast.
        /// </summary>
        [field: SerializeField] public int ScoreAmount { get; private set; }
    }
}