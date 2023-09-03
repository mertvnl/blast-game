using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime.Models
{
    public abstract class ItemData : ScriptableObject
    {
        [field: SerializeField] public ItemBase ItemPrefab { get; private set; }
        [field: SerializeField] public Sprite DefaultSprite { get; private set; }
    }
}