using BlastGame.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class GridTile : MonoBehaviour
    {
        private int _x;
        public int X => _x;
        private int _y;
        public int Y => _y;

        public bool IsEmpty => CurrentItem == null;

        public IItem CurrentItem { get; private set; }

        public void InitializeTile(int x, int y)
        {
            _x = x;
            _y = y;

            gameObject.name = "[" + x + "," + y + "]";
        }

        public void SetItem(IItem item)
        {
            CurrentItem = item;
        }

        public Vector2Int GetGridPosition()
        {
            return new Vector2Int(_x, _y);
        }

        /// <summary>
        /// Returns grid position with grid offset. Helps with item positioning.
        /// </summary>
        public Vector2 GetGridPositionWithOffset()
        {
            return new Vector2(_x + (_x * GridManager.Instance.GRID_OFFSET), _y + (_y * GridManager.Instance.GRID_OFFSET));
        }
    }
}