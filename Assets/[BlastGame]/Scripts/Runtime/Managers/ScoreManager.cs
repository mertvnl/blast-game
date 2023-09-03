using Core.Managers;
using Core.Systems;
using Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        private int _currentScore;
        private int _requiredScore;

        public int CurrentScore => _currentScore;

        public CustomEvent<int> OnScoreChanged { get; private set; } = new();

        private void OnEnable()
        {
            LevelManager.Instance.OnLevelLoaded.AddListener(SetRequiredScore);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelLoaded.RemoveListener(SetRequiredScore);
        }

        public void AddScore(int score)
        {
            _currentScore += score;
            OnScoreChanged.Invoke(_currentScore);
            CheckScore();
        }

        public void SetRequiredScore()
        {
            _requiredScore = LevelManager.Instance.CurrentLevel.LevelData.RequiredScore;
            _currentScore = 0;
        }

        private void CheckScore()
        {
            if (_currentScore < _requiredScore)
                return;

            GameManager.Instance.CompleteLevel(true);
        }
    }
}