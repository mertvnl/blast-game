using Core.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlastGame.Runtime
{
    public class ShuffleButton : MonoBehaviour
    {
        private Button _button;
        public Button Button => _button == null ? _button = GetComponent<Button>() : _button;

        private void OnEnable()
        {
            Button.onClick.AddListener(ShuffleItems);
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(ShuffleItems);
        }

        private void ShuffleItems()
        {
            if (!LevelManager.Instance.IsLevelStarted)
                return;

            ItemManager.Instance.ShuffleItems();
        }
    }
}