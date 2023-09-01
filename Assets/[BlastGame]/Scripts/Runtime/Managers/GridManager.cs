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
    }
}