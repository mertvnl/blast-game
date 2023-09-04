using BlastGame.Interface;
using BlastGame.Runtime.Models;
using Core.Managers;
using Core.Systems;
using Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class ItemManager : Singleton<ItemManager>
    {
        public List<IItem> Items { get; private set; } = new();
        public CustomEvent<List<IItem>> OnItemsBlasted = new();

        private const float SPAWN_OFFSET_Y = 1f;
        private readonly Vector2[] ADJACENT_CHECK_POSITIONS = { Vector2.up , Vector2.down, Vector2.left, Vector2.right };

        private LevelData _levelData;
        private List<ItemData> _chosenItems = new();

        private void OnEnable()
        {
            GridManager.Instance.OnGridInitialized.AddListener(Initialize);
        }

        private void OnDisable()
        {
            GridManager.Instance.OnGridInitialized.RemoveListener(Initialize);
        }

        private void CacheLevelData()
        {
            _levelData = LevelManager.Instance.CurrentLevel.LevelData;
        }

        private void Initialize(int width, int height)
        {
            CacheLevelData();
            InitializeRandomnessBySeed();
            SetChosenItems();
            CreateInitialItems(width, height);
            CheckItemGroups();
        }

        private void SetChosenItems()
        {
            _chosenItems.Clear();

            List<ItemData> items = new(_levelData.BlastableItemDatabase);

            for (int i = 0; i < _levelData.TotalItemColorCount; i++)
            {
                ItemData itemData = items.GetRandom();
                _chosenItems.Add(itemData);
                items.Remove(itemData);
            }
        }

        private void InitializeRandomnessBySeed()
        {
            if (ReferenceEquals(_levelData.LevelSeed, string.Empty))
                return;

            Random.InitState(Animator.StringToHash(_levelData.LevelSeed));
        }

        private void CreateInitialItems(int width, int height)
        {
            Items.Clear();

            if (_levelData.AllowObstacles)
            {
                for (int i = 0; i < _levelData.ObstacleCreationCount; i++)
                {
                    GridTile tile = GridManager.Instance.GetRandomEmptyTile();

                    CreateItem(tile, _levelData.ObstacleItemDatabase.GetRandom());
                }
            }


            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridTile tile = GridManager.Instance.GetTileAtPosition(new Vector2(x, y));

                    if (!tile.IsEmpty)
                        continue;

                    CreateItem(tile, _chosenItems.GetRandom());
                }
            }
        }

        private void CreateItem(GridTile tile, ItemData itemData)
        {
            Vector2 spawnPosition = new(tile.GetGridPosition().x, GridManager.Instance.GetGridSize().y + SPAWN_OFFSET_Y);
            IItem item = Instantiate(itemData.ItemPrefab, spawnPosition, Quaternion.identity, GridManager.Instance.GridRoot);
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

                if (itemGroup.Count < _levelData.BlastableGroupAThreshold)
                    visualType = BlastableVisualType.Default;
                else if (itemGroup.Count >= _levelData.BlastableGroupAThreshold && itemGroup.Count < _levelData.BlastableGroupBThreshold)
                    visualType = BlastableVisualType.A;
                else if (itemGroup.Count >= _levelData.BlastableGroupBThreshold && itemGroup.Count < _levelData.BlastableGroupCThreshold)
                    visualType = BlastableVisualType.B;
                else if (itemGroup.Count >= _levelData.BlastableGroupCThreshold)
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
                CreateItem(emptyTile, _chosenItems.GetRandom());
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

            foreach (IItem adjacentItem in currentMatchedAdjacents)
            {
                if (adjacents.Contains(adjacentItem))
                    continue;

                adjacents.Add(adjacentItem);
                GetAllMatchedAdjacentItems(adjacentItem, ref adjacents);
            }
        }

        public void ShuffleItems()
        {
            List<GridTile> gridTiles = new();

            foreach (IItem item in Items)
                gridTiles.Add(item.CurrentGridTile);

            gridTiles.Shuffle();
            
            for (int i = 0; i < Items.Count; i++)
            {
                IItem item = Items[i];
                item.UpdateGridTile(null);
                item.Move(gridTiles[i].GetGridPosition());
            }

            CheckItemGroups();
        }
    }
}