using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime.Models
{
    [CreateAssetMenu(menuName = "Blast Game/Item/Create Obstacle Item Data")]
    public class ObstacleItemData : ItemData
    {
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public Sprite LowHealthSprite { get; private set; }
    }
}