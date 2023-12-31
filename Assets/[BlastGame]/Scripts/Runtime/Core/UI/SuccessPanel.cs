using Core.Managers;
using Core.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public class SuccessPanel : EasyPanel
    {
        private void OnEnable()
        {
            GameManager.Instance.OnLevelCompleted.AddListener(TogglePanel);
            LevelManager.Instance.OnLevelLoaded.AddListener(HidePanel);
        }

        private void OnDisable()
        {
            GameManager.Instance.OnLevelCompleted.RemoveListener(TogglePanel);
            LevelManager.Instance.OnLevelLoaded.RemoveListener(HidePanel);
        }

        private void Awake()
        {
            HidePanel();
        }

        private void TogglePanel(bool isSuccess)
        {
            if (isSuccess)
                ShowPanelAnimated();
        }

        public void NextLevelButton()
        {
            HidePanelAnimated();
            LevelManager.Instance.LoadNextLevel();
        }
    }
}
