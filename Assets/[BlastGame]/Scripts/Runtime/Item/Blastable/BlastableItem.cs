using BlastGame.Interface;
using BlastGame.Runtime;
using BlastGame.Runtime.Models;
using Core.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class BlastableItem : ItemBase
    {
        private void OnMouseDown()
        {
            CheckIfCanBlast();
        }

        private void CheckIfCanBlast()
        {
            if (!LevelManager.Instance.IsLevelStarted)
                return;

            if (!ItemManager.Instance.IsAdjacentMatching(this))
                return;

            ItemManager.Instance.BlastAllMatches(this);
            MovesManager.Instance.DecreaseMoveCount();
        }

        public override void Initialize(ItemData itemData, GridTile gridTile)
        {
            ItemManager.Instance.AddItem(this);

            base.Initialize(itemData, gridTile);
        }

        public override void Move(GridTile targetGrid, Action onMovementCompleted = null)
        {
            DisableBlasting();
            onMovementCompleted += EnableBlasting;
            base.Move(targetGrid, onMovementCompleted);
            onMovementCompleted -= EnableBlasting;
        }

        public override void Blast()
        {
            base.Blast();
            Dispose();
        }

        private void EnableBlasting()
        {
            CanBlast = true;
        }

        private void DisableBlasting()
        {
            CanBlast = false;
        }

        public override void Dispose()
        {
            ItemManager.Instance.RemoveItem(this);
            base.Dispose();
        }

        public override void SetBlastableGroup(BlastableVisualType visualType)
        {
            BlastableItemData data = ItemData as BlastableItemData;
            SpriteRenderer.sprite = data.GetSpriteByType(visualType);
        }
    }
}