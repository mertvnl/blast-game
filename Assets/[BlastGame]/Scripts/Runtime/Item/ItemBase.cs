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

        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer == null ? _spriteRenderer = GetComponentInChildren<SpriteRenderer>() : _spriteRenderer;

        private const Ease MOVEMENT_EASE = Ease.OutBack;
        private const float MOVEMENT_DURATION = 0.5f;

        public virtual void Initialize(ItemData itemData, GridTile gridTile)
        {
            ItemData = itemData;
            CanMove = true;
            Move(gridTile);
            OnItemDataInitialized.Invoke(itemData);
        }

        public virtual void Blast()
        {
            OnBlasted.Invoke();
        }

        public virtual void Move(GridTile targetGrid, Action onMovementCompleted = null)
        {
            if (!CanMove)
                return;

            UpdateSpriteOrder(targetGrid.Y);
            UpdateGridTile(targetGrid);
            transform.DOLocalMove(targetGrid.GetGridPositionWithOffset(), MOVEMENT_DURATION).SetEase(MOVEMENT_EASE).OnComplete(OnMovementCompleted);

            void OnMovementCompleted()
            {
                onMovementCompleted?.Invoke();
            }
        }
         
        public void UpdateGridTile(GridTile newTile)
        {
            if (newTile == null)
            {
                CurrentGridTile = null;
                return;
            }

            if (CurrentGridTile != null)
                CurrentGridTile.SetItem(null);

            CurrentGridTile = newTile;
            CurrentGridTile.SetItem(this);
        }

        public virtual void Dispose()
        {
            ScoreManager.Instance.AddScore(ItemData.ScoreAmount);
            CurrentGridTile.SetItem(null);
            Instantiate(ItemData.DisposeParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public virtual void SetBlastableGroup(BlastableVisualType visualType) { }

        public void UpdateSpriteOrder(int order)
        {
            SpriteRenderer.sortingOrder = order;
        }
    }
}