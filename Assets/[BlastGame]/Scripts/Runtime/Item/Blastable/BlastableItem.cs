using BlastGame.Interface;
using BlastGame.Runtime;
using BlastGame.Runtime.Models;
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
            if (!ItemManager.Instance.IsAdjacentMatching(this))
            {
                return;
            }

            ItemManager.Instance.BlastAllMatches(this);
        }

        public override void Initialize(ItemData itemData, GridTile gridTile)
        {
            ItemManager.Instance.AddItem(this);

            base.Initialize(itemData, gridTile);
        }

        public override void Move(Vector2 targetPosition, Action onMovementCompleted = null)
        {
            DisableBlasting();
            onMovementCompleted += EnableBlasting;
            base.Move(targetPosition, onMovementCompleted);
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
    }
}