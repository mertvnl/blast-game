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
        ItemData ItemData { get; set; }
        GridTile CurrentGridTile { get; set; }
        CustomEvent<ItemData> OnItemDataInitialized { get; set; }

        void Initialize(ItemData itemData, GridTile gridTile);
        void UpdateGridTile(GridTile newTile);
    }
}