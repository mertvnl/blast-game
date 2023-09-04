using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlastGame.Runtime.Models;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace BlastGame.Tests.EditMode
{
    public class LevelSeedTest
    {
        private const string LEVEL_DATA_PATH = "Assets/[BlastGame]/Data/Level";

        [Test]
        public void LevelSeedTest_IsSeedSame()
        {
            LevelData[] levelDatabase = GetLevelDatabase();

            var duplicates = levelDatabase
            .GroupBy(ld => ld.LevelSeed)
            .Where(group => group.Count() > 1)
            .SelectMany(group => group);

            string msg = "Level seed should not be same with others. ";

            foreach (LevelData duplicate in duplicates)
                msg += "\n" + duplicate.name + " " + duplicate.LevelSeed;

            Assert.AreEqual(false, duplicates.Any(), msg);
        }

        private static LevelData[] GetLevelDatabase()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(LevelData)}", new[] { LEVEL_DATA_PATH });

            return guids.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<LevelData>).ToArray();
        }
    }
}
