using Core.Systems;
using Core.Utilities;
using Sirenix.OdinInspector;

namespace Core.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private bool isLevelCompleted;

        public CustomEvent<bool> OnLevelCompleted = new();

        private void OnEnable()
        {
            LevelManager.Instance.OnLevelLoaded.AddListener(ResetLevelCompletionStatus);
        }

        private void OnDisable()
        {
            LevelManager.Instance.OnLevelLoaded.RemoveListener(ResetLevelCompletionStatus);
        }

        [Button]
        public void CompleteLevel(bool isSuccess)
        {
            if (isLevelCompleted) return;

            isLevelCompleted = true;

            OnLevelCompleted.Invoke(isSuccess);
            LevelManager.Instance.FinishLevel();
            if (isSuccess)
                SaveManager.SetInt("FakeLevel", SaveManager.GetInt("FakeLevel", 1) + 1);
        }

        private void ResetLevelCompletionStatus()
        {
            isLevelCompleted = false;
        }
    }
}
