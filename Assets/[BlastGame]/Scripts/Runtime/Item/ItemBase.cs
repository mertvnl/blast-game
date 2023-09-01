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

        private const Ease MOVEMENT_EASE = Ease.OutBack;
        private const float MOVEMENT_DURATION = 0.5f;

        public abstract void Blast();
        public abstract void Notify(IItem item);


        public virtual void Initialize(ItemData itemData, GridTile gridTile)
        {
            ItemData = itemData;
            CurrentGridTile = gridTile;
            OnItemDataInitialized.Invoke(itemData);
            Move(gridTile.GetPosition().y);
        }

        public void Move(float targetPositionY)
        {
            transform.DOMoveY(targetPositionY, MOVEMENT_DURATION).SetEase(MOVEMENT_EASE).OnComplete(OnMovementCompleted);

            void OnMovementCompleted()
            {
                CurrentGridTile.SetItem(this);
            }
        }
    }
}