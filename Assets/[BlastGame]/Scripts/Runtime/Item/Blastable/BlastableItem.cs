using BlastGame.Interface;
using BlastGame.Runtime;
using BlastGame.Runtime.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class BlastableItem : ItemBase
    {
        public override void Blast()
        {
            ItemManager.Instance.NotifyAdjacents(this);
            Destroy(gameObject);
        }

        public override void Notify(IItem item)
        {
            if (item == null)
                return;

            if (item.ItemData != ItemData)
                return;

            Blast();
        }

        private void OnMouseDown()
        {
            CheckIfCanBlast();
        }

        private void CheckIfCanBlast()
        {
            if (!ItemManager.Instance.IsAdjacentCorresponding(this))
            {

                return;
            }

            Blast();
        }
    }
}