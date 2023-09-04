using BlastGame.Runtime;
using BlastGame.Runtime.Models;
using Core.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Interface
{
    public interface IItem : IBlastable, IMoveable
    {
        ItemData ItemData { get; }
        GridTile CurrentGridTile { get; }
        CustomEvent<ItemData> OnItemDataInitialized { get; }

        /// <summary>
        /// Initialized item by given item data and grid tile.
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="gridTile"></param>
        void Initialize(ItemData itemData, GridTile gridTile);

        /// <summary>
        /// Updates current grid tile of item.
        /// </summary>
        /// <param name="newTile"></param>
        void UpdateGridTile(GridTile newTile);

        void Dispose();
    }
}