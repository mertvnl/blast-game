using BlastGame.Interface;
using BlastGame.Runtime.Models;
using Core.Utilities;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class ItemManager : Singleton<ItemManager>
    {
        [SerializeField] private BlastableItem blastableItem;
        [SerializeField] private List<ItemData> itemDatabase = new();

        private const float SPAWN_OFFSET_Y = 1f;
        private readonly Vector2[] ADJACENT_CHECK_POSITIONS = { Vector2.up , Vector2.down, Vector2.left, Vector2.right };

        private void OnEnable()
        {
            GridManager.Instance.OnGridInitialized.AddListener(CreateItems);
        }

        private void OnDisable()
        {
            GridManager.Instance.OnGridInitialized.RemoveListener(CreateItems);
        }

        private void CreateItems(int width, int height)
        {
            for (int x = 0; x < width;  x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridTile tile = GridManager.Instance.GetTileAtPosition(new Vector2(x, y));
                    
                    if (tile == null)
                        continue;

                    BlastableItem blastable = Instantiate(blastableItem, new Vector3(x, height + SPAWN_OFFSET_Y), Quaternion.identity);
                    blastable.Initialize(itemDatabase.GetRandom(), tile);
                }
            }
        }

        public bool IsAdjacentCorresponding(IItem item)
        {
            return GetMatchedAdjacentItems(item).Count > 0;
        }

        public void NotifyAdjacents(IItem item)
        {
            List<IItem> blastables = GetAdjacentsItems(item);

            if (blastables.Count < 0)
                return;

            foreach (IBlastable blastable in blastables)
                blastable.Notify(item);
        }

        private List<IItem> GetMatchedAdjacentItems(IItem item)
        {
            List<IItem> matchedAdjacents = new();

            foreach (Vector2 adjacentPosition in ADJACENT_CHECK_POSITIONS)
            {
                GridTile tile = GridManager.Instance.GetTileAtPosition(item.CurrentGridTile.GetPosition() + adjacentPosition);

                if (tile == null)
                    continue;

                IItem adjacentItemToCheck = tile.CurrentItem;

                if (adjacentItemToCheck == null)
                    continue;

                if (adjacentItemToCheck.ItemData == item.ItemData)
                    matchedAdjacents.Add(adjacentItemToCheck);
            }

            return matchedAdjacents;
        }

        private List<IItem> GetAdjacentsItems(IItem item)
        {
            List<IItem> adjacents = new();

            foreach (Vector2 adjacentPosition in ADJACENT_CHECK_POSITIONS)
            {
                GridTile tile = GridManager.Instance.GetTileAtPosition(item.CurrentGridTile.GetPosition() + adjacentPosition);

                if (tile == null)
                    continue;

                IItem adjacentItemToCheck = tile.CurrentItem;

                if (adjacentItemToCheck == null)
                    continue;

                adjacents.Add(adjacentItemToCheck);
            }

            return adjacents;
        }
    }
}