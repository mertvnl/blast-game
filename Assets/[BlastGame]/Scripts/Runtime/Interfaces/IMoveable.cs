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
        void Move(Vector2 targetPosition, Action onMovementCompleted = null);
    }
}