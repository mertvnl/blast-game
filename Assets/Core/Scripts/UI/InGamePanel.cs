using Core.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public class InGamePanel : EasyPanel
    {
        private void OnEnable()
        {
            LevelManager.Instance.OnLevelStarted.AddListener(ShowPanelAnimated);
            LevelManager.Instance.OnLevelLoaded.AddListener(HidePanel);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelStarted.RemoveListener(ShowPanelAnimated);
            LevelManager.Instance.OnLevelLoaded.RemoveListener(HidePanel);
        }
    }
}