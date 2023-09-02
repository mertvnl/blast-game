using BlastGame.Interface;
using Core.Managers;
using Core.Systems;
using Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private int width, height;
        [SerializeField] private GridTile gridTilePrefab;
        
        private const float X_OFFSET = -0.15f;
        private const float Y_OFFSET = 1f;

        public CustomEvent<int, int> OnGridInitialized = new();

        private Dictionary<Vector2, GridTile> _tiles;
        public Dictionary<Vector2, GridTile> Tiles => _tiles;

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
            _tiles = new();
            Transform gridRoot = new GameObject("GridRoot").transform;

            for (int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    GridTile instantiatedTile = Instantiate(gridTilePrefab, new Vector3(x, y), Quaternion.identity, gridRoot);
                    instantiatedTile.InitializeTile(x, y);
                    _tiles.Add(new Vector2(x,y), instantiatedTile);
                }
            }

            OnGridInitialized.Invoke(width, height);
        }

        public GridTile GetTileAtPosition(Vector2 position)
        {
            if (_tiles.TryGetValue(position, out GridTile tile))
                return tile;

            return null;
        }

        public GridTile GetLowestEmptyTileAtRow(int x)
        {
            for (int y = 0; y < height; y++)
            {
                GridTile tile = GetTileAtPosition(new Vector2(x, y));

                if (tile == null)
                    continue;

                if (!tile.IsEmpty)
                    continue;

                return tile;
            }

            return null;
        }

        public List<GridTile> GetEmptyTiles()
        {
            List<GridTile> emptyTiles = new();

            for (int x = width - 1; x >= 0; x--)
            {
                for (int y = height - 1; y >= 0; y--)
                {
                    GridTile tile = GetTileAtPosition(new Vector2(x, y));

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

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
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
            return new Vector2(width, height);
        }
    }
}