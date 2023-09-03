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
        [SerializeField] private List<ItemData> itemDatabase = new();

        private int _groupAThreshold = 3;
        private int _groupBThreshold = 4;
        private int _groupCThreshold = 5;

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
            Items.Clear();

            for (int x = 0; x < width;  x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridTile tile = GridManager.Instance.GetTileAtPosition(new Vector2(x, y));
                    
                    if (tile == null)
                        continue;

                    CreateItem(tile);
                }
            }

            CheckItemGroups();
        }

        private void CreateItem(GridTile tile, bool allowObstacles = true)
        {
            List<ItemData> itemDatas = new(itemDatabase);
            
            if (!allowObstacles)
                itemDatas.RemoveAll(data => data is ObstacleItemData);

            ItemData itemData = itemDatas.GetRandom();

            Vector2 spawnPosition = new(tile.GetGridPosition().x, GridManager.Instance.GetGridSize().y + SPAWN_OFFSET_Y);
            IItem item = Instantiate(itemData.ItemPrefab, spawnPosition, Quaternion.identity);
            item.Initialize(itemData, tile);
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

        public bool IsAdjacent(IItem targetItem, List<IItem> itemsToCheck)
        {
            foreach (IItem item in itemsToCheck)
            {
                foreach (Vector2 adjacentPosition in ADJACENT_CHECK_POSITIONS)
                {
                    if(item.CurrentGridTile.GetGridPosition() == targetItem.CurrentGridTile.GetGridPosition() + adjacentPosition)
                        return true;
                }
            }

            return false;
        }

        public void BlastAllMatches(IItem item)
        {
            List<IItem> itemsToBlast = GetAllMatchedAdjacentItems(item);

            if (itemsToBlast.Any(x => !x.CanBlast))
                return;

            foreach (IBlastable blastable in itemsToBlast)
                blastable.Blast();

            UpdateGridItems();

            OnItemsBlasted.Invoke(itemsToBlast);
        }

        [Button]
        public void UpdateGridItems()
        {
            MoveItemsToEmptyTiles();
            RefillTilesWithItems();
            CheckItemGroups();
        }

        private void CheckItemGroups()
        {
            List<IItem> items = new(Items);

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                    continue;

                BlastableVisualType visualType = BlastableVisualType.Default;
                List<IItem> itemGroup = GetAllMatchedAdjacentItems(items[i]);

                if (itemGroup.Count < _groupAThreshold)
                    visualType = BlastableVisualType.Default;
                else if (itemGroup.Count >= _groupAThreshold && itemGroup.Count < _groupBThreshold)
                    visualType = BlastableVisualType.A;
                else if (itemGroup.Count >= _groupBThreshold && itemGroup.Count < _groupCThreshold)
                    visualType = BlastableVisualType.B;
                else if (itemGroup.Count >= _groupCThreshold)
                    visualType = BlastableVisualType.C;

                foreach (var item in itemGroup)
                    item.SetBlastableGroup(visualType);
            }
        }

        private void MoveItemsToEmptyTiles()
        {
            List<GridTile> tiles = GridManager.Instance.GetTilesWithItem();

            foreach (GridTile tile in tiles)
            {
                Vector2 positionToCheck = tile.GetGridPosition() + Vector2.down;

                GridTile tileAtBelow = GridManager.Instance.GetTileAtPosition(positionToCheck);

                if (tileAtBelow == null)
                    continue;

                if (!tileAtBelow.IsEmpty)
                    continue;

                GridTile lowestTile = GridManager.Instance.GetLowestEmptyTileAtRow(tile.GetGridPosition());

                if (lowestTile == null)
                    continue;

                tile.CurrentItem.Move(lowestTile.GetGridPosition());
            }
        }

        private void RefillTilesWithItems()
        {
            List<GridTile> emptyTiles = GridManager.Instance.GetFillableTiles();

            foreach (GridTile emptyTile in emptyTiles)
                CreateItem(emptyTile, false);
        }

        private List<IItem> GetMatchedAdjacentItems(IItem item)
        {
            List<IItem> matchedAdjacents = new();

            foreach (Vector2 adjacentPosition in ADJACENT_CHECK_POSITIONS)
            {
                GridTile tile = GridManager.Instance.GetTileAtPosition(item.CurrentGridTile.GetGridPosition() + adjacentPosition);

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