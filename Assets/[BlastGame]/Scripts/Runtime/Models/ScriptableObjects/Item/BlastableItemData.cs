using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime.Models
{
    [CreateAssetMenu(menuName = "Blast Game/Item/Create Blastable Item Data")]
    public class BlastableItemData : ItemData
    {
        [field: SerializeField] BlastableVisualDictionary BlastableVisualDictionary { get; set; } = new();

        public Sprite GetSpriteByType(BlastableVisualType type)
        {
            if (BlastableVisualDictionary.TryGetValue(type, out Sprite sprite))
                return sprite;

            return DefaultSprite;
        }
    }

    [Serializable]
    public class BlastableVisualDictionary : SerializableDictionary<BlastableVisualType, Sprite> { }
}