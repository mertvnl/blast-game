using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public class TapToPlayAnimation : MonoBehaviour
    {
        private void Awake()
        {
            transform.DOPunchScale(Vector2.one * 0.1f, 1f, 1, 1).SetLoops(-1);
        }
    }
}