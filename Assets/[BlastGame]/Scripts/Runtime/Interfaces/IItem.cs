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

        void Initialize(ItemData itemData, GridTile gridTile);
        void UpdateGridTile(GridTile newTile);
        void Dispose();
    }
}