using BlastGame.Interface;
using BlastGame.Runtime.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class BoxObstacleHealthController : DamageableBase
    {
        private IItem _item;
        public IItem Item => _item == null ? _item = GetComponent<IItem>() : _item;

        private void OnEnable()
        {
            Item.OnItemDataInitialized.AddListener(GetAndSetHealthData);
            Item.OnBlasted.AddListener(BlastDamage);
            OnHealthZeroed.AddListener(Item.Dispose);
        }

        private void OnDisable()
        {
            Item.OnItemDataInitialized.RemoveListener(GetAndSetHealthData);
            Item.OnBlasted.RemoveListener(BlastDamage);
            OnHealthZeroed.RemoveListener(Item.Dispose);
        }

        private void GetAndSetHealthData(ItemData data)
        {
            SetInitialHealth((data as ObstacleItemData).Health);
        }

        private void BlastDamage()
        {
            TakeDamage();
        }
    }
}
