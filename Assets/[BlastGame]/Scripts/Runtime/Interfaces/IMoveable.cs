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
        void Move(GridTile targetGrid, Action onMovementCompleted = null);
        void UpdateSpriteOrder(int order);
    }
}