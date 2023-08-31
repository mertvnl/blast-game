using Core.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class RestartButton : MonoBehaviour
    {
        private Button button;
        public Button Button { get { return button == null ? button = GetComponent<Button>() : button; } }

        private void OnEnable()
        {
            Button.onClick.AddListener(RestartLevel);
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(RestartLevel);
        }

        private void RestartLevel()
        {
            LevelManager.Instance.RestartLevel();
        }
    }
}