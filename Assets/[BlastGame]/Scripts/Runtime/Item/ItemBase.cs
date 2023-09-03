using BlastGame.Interface;
using BlastGame.Runtime.Models;
using Core.Systems;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public abstract class ItemBase : MonoBehaviour, IItem
    {
        public ItemData ItemData { get; set; }
        public GridTile CurrentGridTile { get; set; }
        public CustomEvent<ItemData> OnItemDataInitialized { get; private set; } = new();
        public CustomEvent OnBlasted { get; private set; } = new();
        public bool CanBlast { get; set; }
        public bool CanMove { get; set; }

        private const Ease MOVEMENT_EASE = Ease.OutBack;
        private const float MOVEMENT_DURATION = 0.5f;

        public virtual void Initialize(ItemData itemData, GridTile gridTile)
        {
            ItemData = itemData;
            CanMove = true;
            Move(gridTile.GetGridPosition());
            OnItemDataInitialized.Invoke(itemData);
        }

        public virtual void Blast()
        {
            OnBlasted.Invoke();
        }

        public virtual void Move(Vector2 targetPosition, Action onMovementCompleted = null)
        {
            if (!CanMove)
                return;

            UpdateGridTile(GridManager.Instance.GetTileAtPosition(targetPosition));
            transform.DOMove(targetPosition, MOVEMENT_DURATION).SetEase(MOVEMENT_EASE).OnComplete(OnMovementCompleted);

            void OnMovementCompleted()
            {
                onMovementCompleted?.Invoke();
            }
        }

        public void UpdateGridTile(GridTile newTile)
        {
            if (CurrentGridTile != null)
                CurrentGridTile.SetItem(null);

            CurrentGridTile = newTile;
            CurrentGridTile.SetItem(this);
        }

        public virtual void Dispose()
        {
            CurrentGridTile.SetItem(null);
            Destroy(gameObject);
        }

        public virtual void SetBlastableGroup(BlastableVisualType visualType) { }
    }
}