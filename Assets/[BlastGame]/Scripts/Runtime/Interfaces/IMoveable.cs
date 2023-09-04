using BlastGame.Runtime;
using Core.Utilities;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Interface
{
    public interface IMoveable : IComponent
    {
        bool CanMove { get; }

        /// <summary>
        /// Moves the object towards the target grid tile.
        /// </summary>
        /// <param name="targetGrid"></param>
        /// <param name="onMovementCompleted">After movement completed this action is invoked.</param>
        void Move(GridTile targetGrid, Action onMovementCompleted = null);

        /// <summary>
        /// Updates sprite renderer's sorting order to prevent the sprite overlap.
        /// </summary>
        /// <param name="order"></param>
        void UpdateSpriteOrder(int order);
    }
}