using BlastGame.Interface;
using BlastGame.Runtime.Models;
using Core.Systems;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public abstract class ItemBase : MonoBehaviour, IItem
    {
        public ItemData ItemData { get; set; }
        public GridTile CurrentGridTile { get; set; }
        public CustomEvent<ItemData> OnItemDataInitialized { get; set; } = new();
        public bool CanBlast { get; private set; }

        private const Ease MOVEMENT_EASE = Ease.OutBack;
        private const float MOVEMENT_DURATION = 0.5f;

        public virtual void Initialize(ItemData itemData, GridTile gridTile)
        {
            ItemManager.Instance.AddItem(this);
            ItemData = itemData;
            Move(gridTile.GetPosition());
            OnItemDataInitialized.Invoke(itemData);
        }

        public virtual void Blast()
        {
            ItemManager.Instance.RemoveItem(this);
            CurrentGridTile.SetItem(null);
            Destroy(gameObject);
        }

        public virtual void Move(Vector2 targetPosition)
        {
            UpdateGridTile(GridManager.Instance.GetTileAtPosition(targetPosition));
            CanBlast = false;
            transform.DOMove(targetPosition, MOVEMENT_DURATION).SetEase(MOVEMENT_EASE).OnComplete(OnMovementCompleted);

            void OnMovementCompleted()
            {
                CanBlast = true;
            }
        }

        public void UpdateGridTile(GridTile newTile)
        {
            if (CurrentGridTile != null)
                CurrentGridTile.SetItem(null);

            CurrentGridTile = newTile;
            CurrentGridTile.SetItem(this);
        }
    }
}