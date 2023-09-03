using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BlastGame.Runtime
{
    public class MovesTextController : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        public TextMeshProUGUI Text => _text == null ? _text = GetComponent<TextMeshProUGUI>() : _text;

        private void OnEnable()
        {
            MovesManager.Instance.OnInitialMoveCountSet.AddListener(SetMoveText);
            MovesManager.Instance.OnMoveCountChanged.AddListener(SetMoveText);
        }

        private void OnDisable()
        {
            MovesManager.Instance.OnInitialMoveCountSet.RemoveListener(SetMoveText);
            MovesManager.Instance.OnMoveCountChanged.RemoveListener(SetMoveText);
        }

        private void SetMoveText(int moveCount)
        {
            Text.SetText(moveCount.ToString());
        }
    }
}