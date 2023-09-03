using BlastGame.Interface;
using BlastGame.Runtime.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class BoxObstacleItem : ItemBase
    {
        private void OnEnable()
        {
            ItemManager.Instance.OnItemsBlasted.AddListener(CheckIfAdjacent);
        }

        private void OnDisable()
        {
            ItemManager.Instance.OnItemsBlasted.RemoveListener(CheckIfAdjacent);
        }

        public override void Initialize(ItemData itemData, GridTile gridTile)
        {
            base.Initialize(itemData, gridTile);
            CanBlast = true;
            CanMove = false;
        }

        private void CheckIfAdjacent(List<IItem> blastedItems)
        {
            if (!ItemManager.Instance.IsAdjacent(this, blastedItems))
                return;

            Blast();
        }

        public override void Blast()
        {
            base.Blast();
            ItemManager.Instance.UpdateGridItems();
        }
    }
}