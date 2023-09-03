using BlastGame.Interface;
using BlastGame.Runtime.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class BoxObstacleVisualController : MonoBehaviour
    {
        private IDamageable _damageable;
        public IDamageable Damageable => _damageable == null ? _damageable = GetComponent<IDamageable>() : _damageable;

        private IItem _item;
        public IItem Item => _item == null ? _item = GetComponent<IItem>() : _item;

        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer == null ? _spriteRenderer = GetComponentInChildren<SpriteRenderer>() : _spriteRenderer;

        private ObstacleItemData _obstacleItemData;

        private int _initialHealth;

        private void OnEnable()
        {
            Item.OnItemDataInitialized.AddListener(Initialize);
            Damageable.OnHealthSet.AddListener(CacheInitialHealth);
            Damageable.OnHealthChanged.AddListener(CheckIfHealthIsHalf);
        }

        private void OnDisable()
        {
            Item.OnItemDataInitialized.RemoveListener(Initialize);
            Damageable.OnHealthSet.RemoveListener(CacheInitialHealth);
            Damageable.OnHealthChanged.RemoveListener(CheckIfHealthIsHalf);
        }

        private void Initialize(ItemData data)
        {
            _obstacleItemData = data as ObstacleItemData;

            UpdateVisual(false);
        }

        private void CacheInitialHealth(int initialHealth)
        {
            _initialHealth = initialHealth;
        }

        private void CheckIfHealthIsHalf(int currentHealth)
        {
            if (currentHealth > _initialHealth / 2)
                return;

            UpdateVisual(true);
        }

        private void UpdateVisual(bool isHalf)
        {
            SpriteRenderer.sprite = isHalf ? _obstacleItemData.LowHealthSprite : _obstacleItemData.DefaultSprite;
        }
    }
}