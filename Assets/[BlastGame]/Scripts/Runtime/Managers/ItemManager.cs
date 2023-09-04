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

        private LevelData _levelData;
        private List<ItemData> _chosenItems = new();

        private const float INITIAL_SPAWN_OFFSET_Y = 1f;
        private readonly Vector2Int[] ADJACENT_CHECK_POSITIONS = { Vector2Int.up , Vector2Int.down, Vector2Int.left, Vector2Int.right };

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

        /// <summary>
        /// Gets random items by given count from level data.
        /// </summary>
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

        /// <summary>
        /// Initalizes randomness in order to have random but unique item creation randomness.
        /// This is helpful because if player fails level X, when it is played again level it will be same level.
        /// Also every device will have same randomness.
        /// </summary>
        private void InitializeRandomnessBySeed()
        {
            if (ReferenceEquals(_levelData.LevelSeed, string.Empty))
                return;

            Random.InitState(Animator.StringToHash(_levelData.LevelSeed));
        }

        /// <summary>
        /// Creates starting items and obstacles by given width and height.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
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
                    GridTile tile = GridManager.Instance.GetTileAtPosition(new Vector2Int(x, y));

                    if (!tile.IsEmpty)
                        continue;

                    CreateItem(tile, _chosenItems.GetRandom());
                }
            }
        }

        /// <summary>
        /// Creates an item and initalizes it.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="itemData"></param>
        private void CreateItem(GridTile tile, ItemData itemData)
        {
            Vector2 spawnPosition = new(tile.GetGridPositionWithOffset().x, GridManager.Instance.GetGridSize().y + INITIAL_SPAWN_OFFSET_Y);
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

        /// <summary>
        /// Checks if given item's adjacents are same or not.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsAdjacentMatching(IItem item)
        {
            return GetMatchedAdjacentItems(item).Count > 0;
        }

        /// <summary>
        /// Checks if given item is adjacent of given items from list.
        /// </summary>
        /// <param name="targetItem"></param>
        /// <param name="itemsToCheck"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Blasts group if items by given item.
        /// </summary>
        /// <param name="item"></param>
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

        /// <summary>
        /// Updates grid items by moving items into empty tiles, refilling tiles and checking item groups.
        /// </summary>
        public void UpdateGridItems()
        {
            MoveItemsToEmptyTiles();
            RefillTilesWithItems();
            CheckItemGroups();
        }

        /// <summary>
        /// Checks current items' group count and sets their blastable group.
        /// </summary>
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

        /// <summary>
        /// Moves items into empty tiles.
        /// </summary>
        private void MoveItemsToEmptyTiles()
        {
            List<GridTile> tiles = GridManager.Instance.GetTilesWithItem();

            foreach (GridTile tile in tiles)
            {
                Vector2Int positionToCheck = tile.GetGridPosition() + Vector2Int.down;

                GridTile tileAtBelow = GridManager.Instance.GetTileAtPosition(positionToCheck);

                if (tileAtBelow == null)
                    continue;

                if (!tileAtBelow.IsEmpty)
                    continue;

                GridTile lowestTile = GridManager.Instance.GetLowestEmptyTileAtRow(tile.GetGridPosition());

                if (lowestTile == null)
                    continue;

                tile.CurrentItem.Move(lowestTile);
            }
        }

        /// <summary>
        /// Refills empty tiles.
        /// </summary>
        private void RefillTilesWithItems()
        {
            List<GridTile> emptyTiles = GridManager.Instance.GetFillableTiles();

            foreach (GridTile emptyTile in emptyTiles)
                CreateItem(emptyTile, _chosenItems.GetRandom());
        }

        /// <summary>
        /// Checks adjacent of given item. Then returns any.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private List<IItem> GetMatchedAdjacentItems(IItem item)
        {
            List<IItem> matchedAdjacents = new();

            foreach (Vector2Int adjacentPosition in ADJACENT_CHECK_POSITIONS)
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

        /// <summary>
        /// Gets all blastable group of given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private List<IItem> GetAllMatchedAdjacentItems(IItem item)
        {
            List<IItem> allMatchedAdjacentItems = new() { item };

            GetAllMatchedAdjacentItems(item, ref allMatchedAdjacentItems);

            return allMatchedAdjacentItems;
        }

        /// <summary>
        /// Recursive method to get all matched adjacent items.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="adjacents"></param>
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

        /// <summary>
        /// Shuffles items and moves them to their new grid tiles. Then checks for item groups.
        /// </summary>
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
                item.Move(gridTiles[i]);
            }

            CheckItemGroups();
        }
    }
}