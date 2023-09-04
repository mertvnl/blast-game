using Core.Managers;
using Core.Systems;
using Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class MovesManager : Singleton<MovesManager>
    {
        private int _moveCount = 0;

        public CustomEvent<int> OnMoveCountChanged { get; private set; } = new();
        public CustomEvent<int> OnInitialMoveCountSet { get; private set; } = new();

        private void OnEnable()
        {
            LevelManager.Instance.OnLevelLoaded.AddListener(SetMoveCount);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelLoaded.RemoveListener(SetMoveCount);
        }

        private void SetMoveCount()
        {
            _moveCount = LevelManager.Instance.CurrentLevel.LevelData.MoveCount;
            OnInitialMoveCountSet.Invoke(_moveCount);
        }

        public void DecreaseMoveCount()
        {
            _moveCount--;
            OnMoveCountChanged.Invoke(_moveCount);
            CheckMoveCount();
        }

        /// <summary>
        /// Checks move count, if is player out of move, completes level as a failure.
        /// </summary>
        private void CheckMoveCount()
        {
            if (_moveCount > 0)
                return;

            GameManager.Instance.CompleteLevel(false);
        }
    }
}