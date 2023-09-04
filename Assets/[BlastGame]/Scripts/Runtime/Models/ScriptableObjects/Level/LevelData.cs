using Sirenix.OdinInspector;
using System;
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

        /// <summary>
        /// Allows or disallows obstacle creation for the level.
        /// </summary>
        [Space(10)]
        [BoxGroup("Item Settings")]
        public bool AllowObstacles;

        /// <summary>
        /// Total different item color that is going to be created for the level.
        /// </summary>
        [Space(10)]
        [InfoBox("Total unique item type to create.")]
        [BoxGroup("Item Settings")]
        [PropertyRange(1, "MaxBlastableCount")]
        public int TotalItemColorCount;

        /// <summary>
        /// Obstacle count to create.
        /// </summary>
        [Space(10)]
        [InfoBox("Amount of obstacles to create while initializing the items.")]
        [BoxGroup("Item Settings")]
        [ShowIf("AllowObstacles")]
        [Range(0, 100)]
        public int ObstacleCreationCount;

        /// <summary>
        /// Blastable group thresholds to determine blastable group sprite.
        /// </summary>
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

        /// <summary>
        /// Required score to complete level.
        /// </summary>
        [BoxGroup("Level Settings")]
        [InfoBox("Target score required to win the level.")]
        [Range(100, 1000000)]
        public int RequiredScore;

        /// <summary>
        /// Total move count that can player make for the level.
        /// </summary>
        [BoxGroup("Level Settings")]
        [InfoBox("Move count that player can make in this level.")]
        [Range(1, 1000)]
        public int MoveCount;

        /// <summary>
        /// Level seed to initialize level randomness.
        /// </summary>
        [BoxGroup("Level Settings")]
        [InfoBox("Level seed initializes randomness in order to generate same level items for every device.")]
        [ReadOnly]
        public string LevelSeed;

        public int MaxBlastableCount => BlastableItemDatabase.Count;

        [Button]
        private void GenerateLevelSeed()
        {
            LevelSeed = Guid.NewGuid().ToString();
        }
    }
}