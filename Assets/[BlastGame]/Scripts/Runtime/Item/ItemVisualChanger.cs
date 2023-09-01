using BlastGame.Interface;
using BlastGame.Runtime.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class ItemVisualChanger : MonoBehaviour
    {
        private IItem _item;
        public IItem Item => _item == null ? _item = GetComponent<IItem>() : _item;

        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer == null ? _spriteRenderer = GetComponentInChildren<SpriteRenderer>() : _spriteRenderer;

        private void OnEnable()
        {
            Item.OnItemDataInitialized.AddListener(SetVisual);
        }

        private void OnDisable()
        {
            Item.OnItemDataInitialized.RemoveListener(SetVisual);
        }

        private void SetVisual(ItemData data)
        {
            SpriteRenderer.sprite = data.DefaultSprite;
        }
    }
}