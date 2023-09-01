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

        private void OnMouseDown()
        {
            CheckIfCanBlast();
        }

        private void CheckIfCanBlast()
        {
            if (!ItemManager.Instance.IsAdjacentMatching(this))
            {
                return;
            }

            ItemManager.Instance.BlastAllMatches(this);
        }
    }
}