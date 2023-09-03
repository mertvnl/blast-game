using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime.Models
{
    [CreateAssetMenu(menuName = "Blast Game/Level/Create New Level Data")]
    public class LevelData : ScriptableObject
    {
        [BoxGroup("Grid Settings")]
        public GridData GridData;

        [Space(10)]
        [BoxGroup("Item Settings")]
        public bool AllowObstacles;

        [Space(10)]
        [InfoBox("Total unique item type to create.")]
        [BoxGroup("Item Settings")]
        [PropertyRange(1, "MaxBlastableCount")]
        public int TotalItemColorCount;

        [Space(10)]
        [InfoBox("Amount of obstacles to create while initializing the items.")]
        [BoxGroup("Item Settings")]
        [ShowIf("AllowObstacles")]
        [Range(0, 10)]
        public int ObstacleCreationCount;

        [Space(10)]
        [InfoBox("Blastable group threshold to determine the icons.")]
        [BoxGroup("Item Settings")]
        public int BlastableGroupAThreshold;
        [BoxGroup("Item Settings")]
        public int BlastableGroupBThreshold;
        [BoxGroup("Item Settings")]
        public int BlastableGroupCThreshold;

        [Space(10)]
        [BoxGroup("Item Settings")]
        public List<ItemData> BlastableItemDatabase = new();

        [BoxGroup("Item Settings")]
        [ShowIf("AllowObstacles")]
        public List<ItemData> ObstacleItemDatabase = new();

        [BoxGroup("Level Settings")]
        [InfoBox("Target score required to win the level.")]
        public int RequiredScore;

        public int MaxBlastableCount => BlastableItemDatabase.Count;
    }
}