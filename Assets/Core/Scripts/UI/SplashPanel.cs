using Core.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public class SplashPanel : EasyPanel
    {
        private void OnEnable()
        {
            LevelManager.Instance.OnLevelLoaded.AddListener(OnLevelLoaded);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelLoaded.RemoveListener(OnLevelLoaded);
        }

        private void Awake()
        {
            ShowPanel();
        }

        private void OnLevelLoaded()
        {
            HidePanel();
            Destroy(gameObject);
        }
    }
}
