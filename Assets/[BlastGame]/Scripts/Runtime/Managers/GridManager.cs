using BlastGame.Runtime.Models;
using Core.Managers;
using Core.Systems;
using Core.Utilities;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class GridManager : Singleton<GridManager>
    {
        private Dictionary<Vector2, GridTile> _tiles;
        public Dictionary<Vector2, GridTile> Tiles => _tiles;

        public Transform GridRoot { get; private set; }
        public CustomEvent<int, int> OnGridInitialized = new();

        private GridData _gridData;

        public readonly float GRID_OFFSET = -0.15f;

        private void OnEnable()
        {
            LevelManager.Instance.OnLevelLoaded.AddListener(InitializeGrid);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelLoaded.RemoveListener(InitializeGrid);
        }

        public void InitializeGrid()
        {
            _gridData = LevelManager.Instance.CurrentLevel.LevelData.GridData;

            _tiles = new();

            CreateGridRoot();

            for (int x = 0; x < _gridData.Width; x++)
            {
                for(int y = 0; y < _gridData.Height; y++)
                {
                    GridTile instantiatedTile = Instantiate(_gridData.GridTilePrefab, new Vector3(x, y), Quaternion.identity, GridRoot);
                    instantiatedTile.InitializeTile(x, y);
                    _tiles.Add(new Vector2(x,y), instantiatedTile);
                }
            }


            OnGridInitialized.Invoke(_gridData.Width, _gridData.Height);
        }

        private void CreateGridRoot()
        {
            GridRoot = new GameObject(nameof(GridRoot)).transform;
            GameObject GridVisualObject = new(nameof(GridVisualObject));
            GridVisualObject.transform.SetParent(GridRoot);
            GridVisualObject.transform.position = new Vector2((float)_gridData.Width / 2 - 0.5f + (((float)_gridData.Width / 2 - 0.5f) * GRID_OFFSET), (float)_gridData.Height / 2 - 0.5f + (((float)_gridData.Height / 2 - 0.5f) * GRID_OFFSET));
            GridRoot.transform.position = new Vector3(10, GridRoot.transform.position.y);
            SpriteRenderer spriteRenderer = GridVisualObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = -5;
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            spriteRenderer.sprite = _gridData.GridSprite;
            spriteRenderer.size = new(_gridData.Width + (_gridData.Width * GRID_OFFSET) + 0.55f, _gridData.Height + (_gridData.Height * GRID_OFFSET) + 0.60f);
            GridRoot.transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);
        }

        public GridTile GetTileAtPosition(Vector2 position)
        {
            if (_tiles.TryGetValue(position, out GridTile tile))
                return tile;

            return null;
        }

        public GridTile GetLowestEmptyTileAtRow(Vector2 position)
        {
            int targetHeight = (int)position.y;
            int startHeight = 0;

            for (int y = targetHeight; y > -1; y--)
            {
                GridTile tile = GetTileAtPosition(new Vector2(position.x, y));

                if (tile == null)
                    continue;

                if (!tile.IsEmpty && !tile.CurrentItem.CanMove)
                {
                    startHeight = y;
                    break;
                }
            }

            for (int y = startHeight; y < targetHeight; y++)
            {
                GridTile tile = GetTileAtPosition(new Vector2(position.x, y));

                if (tile == null)
                    continue;

                if (!tile.IsEmpty)
                    continue;

                return tile;
            }

            return null;
        }

        public List<GridTile> GetFillableTiles()
        {
            List<GridTile> emptyTiles = new();

            for (int x = 0; x < _gridData.Width; x++)
            {
                for (int y = _gridData.Height - 1; y >= 0; y--)
                {
                    GridTile tile = GetTileAtPosition(new Vector2(x, y));

                    if (!tile.IsEmpty && !tile.CurrentItem.CanMove)
                        break;

                    if (!tile.IsEmpty)
                        continue;

                    emptyTiles.Add(tile);
                }
            }

            return emptyTiles;
        }

        public List<GridTile> GetTilesWithItem()
        {
            List<GridTile> tilesWithItem = new();

            for (int x = 0; x < _gridData.Width; x++)
            {
                for (int y = 0; y < _gridData.Height; y++)
                {
                    GridTile tile = GetTileAtPosition(new Vector2(x, y));

                    if (tile.IsEmpty)
                        continue;

                    tilesWithItem.Add(tile);
                }
            }

            return tilesWithItem;
        }

        public Vector2 GetGridSize()
        {
            return new Vector2(_gridData.Width, _gridData.Height);
        }

        public GridTile GetRandomEmptyTile()
        {
            List<GridTile> emptyTiles = Tiles.Values.Where(tile => tile.IsEmpty).ToList();

            if (emptyTiles.Count == 0 || emptyTiles == null)
                return null;

            return emptyTiles.GetRandom();
        }
    }
}