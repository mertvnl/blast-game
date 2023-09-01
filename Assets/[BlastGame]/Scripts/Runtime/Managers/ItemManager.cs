using BlastGame.Interface;
using BlastGame.Runtime.Models;
using Core.Systems;
using Core.Utilities;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class ItemManager : Singleton<ItemManager>
    {
        [SerializeField] private BlastableItem blastableItem;
        [SerializeField] private List<ItemData> itemDatabase = new();

        public List<IItem> Items { get; private set; } = new();
        public CustomEvent<List<IItem>> OnItemsBlasted = new();

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

        public void AddItem(IItem item)
        {
            if (Items.Contains(item))
                return;

            Items.Add(item);
        }

        public void RemoveItem(IItem item)
        {
            if (!Items.Contains(item))
                return;

            Items.Remove(item);
        }

        public bool IsAdjacentMatching(IItem item)
        {
            return GetMatchedAdjacentItems(item).Count > 0;
        }

        public void BlastAllMatches(IItem item)
        {
            List<IItem> itemsToBlast = GetAllMatchedAdjacentItems(item);

            if (itemsToBlast.Any(x => !x.CanBlast))
                return;

            foreach (IBlastable blastable in itemsToBlast)
                blastable.Blast();

            MoveItemsToEmptyTiles();



            OnItemsBlasted.Invoke(itemsToBlast);
        }

        private void MoveItemsToEmptyTiles()
        {
            List<IItem> items = new(Items);

            foreach (IItem item in items)
            {
                Vector2 positionToCheck = item.CurrentGridTile.GetPosition() + Vector2.down;

                GridTile tileAtBelow = GridManager.Instance.GetTileAtPosition(positionToCheck);

                if (tileAtBelow == null)
                    continue;

                if (!tileAtBelow.IsEmpty)
                    continue;

                GridTile lowestTile = GridManager.Instance.GetLowestEmptyTileAtRow((int)positionToCheck.x);

                if (lowestTile == null)
                    continue;

                item.Move(lowestTile.GetPosition());
            }
        }

        private void RefillTilesWithItems()
        {

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

        private List<IItem> GetAllMatchedAdjacentItems(IItem item)
        {
            List<IItem> allMatchedAdjacentItems = new() { item };

            GetAllMatchedAdjacentItems(item, ref allMatchedAdjacentItems);

            return allMatchedAdjacentItems;
        }

        private void GetAllMatchedAdjacentItems(IItem item, ref List<IItem> adjacents)
        {
            List<IItem> currentMatchedAdjacents = GetMatchedAdjacentItems(item);

            if (currentMatchedAdjacents.Count == 0)
                return;

            foreach (var adjacentItem in currentMatchedAdjacents)
            {
                if (adjacents.Contains(adjacentItem))
                    continue;

                adjacents.Add(adjacentItem);
                GetAllMatchedAdjacentItems(adjacentItem, ref adjacents);
            }
        }
    }
}