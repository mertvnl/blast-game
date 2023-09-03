using Core.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class ScoreTextController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentScoreTMP;
        [SerializeField] private TextMeshProUGUI _targetScoreTMP;

        private void OnEnable()
        {
            LevelManager.Instance.OnLevelLoaded.AddListener(SetTargetScoreText);
            ScoreManager.Instance.OnScoreChanged.AddListener(SetScoreText);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelLoaded.RemoveListener(SetTargetScoreText);
            ScoreManager.Instance.OnScoreChanged.RemoveListener(SetScoreText);
        }

        private void SetTargetScoreText()
        {
            _targetScoreTMP.SetText(LevelManager.Instance.CurrentLevel.LevelData.RequiredScore.ToString());
            _currentScoreTMP.SetText(0.ToString());
        }

        private void SetScoreText(int score)
        {
            _currentScoreTMP.SetText(score.ToString());
        }
    }
}