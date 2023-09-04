using BlastGame.Interface;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class BoxObstacleAnimation : MonoBehaviour
    {
        private IDamageable _damageable;
        private IDamageable Damageable => _damageable == null ? _damageable = GetComponent<IDamageable>() : _damageable;

        [SerializeField] private Transform visualTransform;

        private Tween _scaleTween;
        private Tween _rotationTween;

        private const float ANIMATION_DURATION = 0.5f;
        private const float ANIMATION_SCALE_MULTIPLIER = 0.1f;
        private const float ANIMATION_ROTATION_MULTIPLIER = 15f;
        
        private void OnEnable()
        {
            Damageable.OnHealthChanged.AddListener(CheckHealth);
        }

        private void OnDisable()
        {
            Damageable.OnHealthChanged.RemoveListener(CheckHealth);
        }

        private void CheckHealth(int health)
        {
            if (health <= 0)
                return;

            ShakeAnimation();
        }

        private void ShakeAnimation()
        {
            _scaleTween?.Kill(true);
            _scaleTween = transform.DOPunchScale(Vector3.one * ANIMATION_SCALE_MULTIPLIER, ANIMATION_DURATION);
            _rotationTween?.Kill(true);
            _rotationTween = transform.DOPunchRotation(Vector3.forward * ANIMATION_ROTATION_MULTIPLIER, ANIMATION_DURATION);
        }
    }
}