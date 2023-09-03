using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BlastGame.Runtime.Models;
using Core.Models;
using Core.Systems;
using Core.Utilities;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Core.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private LevelDatabase levelDatabase;

        public Level CurrentLevel { get; private set; }

        public bool IsLevelStarted { get; set; }

        private int _currentLevelIndex;
        private bool _isLevelLoading;
        private bool _canStartLevel;

        public CustomEvent OnLevelLoadingStarted = new();
        public CustomEvent OnLevelLoaded = new();
        public CustomEvent OnLevelStarted = new();
        public CustomEvent OnLevelFinished = new();

        public void StartLevel()
        {
            if (IsLevelStarted) return;

            if (!_canStartLevel) return;

            _canStartLevel = false;
            IsLevelStarted = true;
            OnLevelStarted.Invoke();
        }

        public void FinishLevel()
        {
            if (!IsLevelStarted) return;

            IsLevelStarted = false;
            OnLevelFinished.Invoke();
        }

        private void LoadLevel(int levelIndex)
        {
            if (_isLevelLoading) return;

            StartCoroutine(LoadLevelCo(levelIndex));
        }

        private IEnumerator LoadLevelCo(int levelIndex)
        {
            IsLevelStarted = false;
            _isLevelLoading = true;
            OnLevelLoadingStarted.Invoke();
            yield return new WaitForSeconds(1f);
            Level targetLevel = levelDatabase.GetLevelByIndex(levelIndex);
            yield return SceneManager.LoadSceneAsync(targetLevel.LevelId);
            yield return new WaitForSeconds(0.5f);
            _currentLevelIndex = levelIndex;
            SaveManager.SetInt("LastLevelIndex", _currentLevelIndex);
            CurrentLevel = targetLevel;
            OnLevelLoaded.Invoke();
            _isLevelLoading = false;
            _canStartLevel = true;
        }

        public void LoadLastLevel()
        {
            LoadLevel(GetLastLevelIndex());
        }

        [Button]
        public void LoadNextLevel()
        {
            int nextLevelIndex = _currentLevelIndex + 1;

            if (nextLevelIndex > GetLevelCount())
                LoadLevel(0);
            else
                LoadLevel(nextLevelIndex);
        }

        [Button]
        public void LoadPreviousLevel()
        {
            int previousLevelIndex = _currentLevelIndex - 1;

            if (previousLevelIndex < 0)
                LoadLevel(0);
            else
                LoadLevel(previousLevelIndex);
        }

        [Button]
        public void RestartLevel()
        {
            LoadLevel(_currentLevelIndex);
        }

        public void LoadCurrentEditorLevel()
        {
            _currentLevelIndex = levelDatabase.GetLevelIndexById(SceneManager.GetActiveScene().name);
            CurrentLevel = levelDatabase.GetLevelByIndex(_currentLevelIndex);
            OnLevelLoaded.Invoke();
            _canStartLevel = true;
        }

        public int GetLevelCount()
        {
            return levelDatabase.levels.Length - 1;
        }

        public int GetLastLevelIndex()
        {
            return SaveManager.GetInt("LastLevelIndex", 0);
        }
    }

    public enum LevelType
    {
        Normal,
        Tutorial
    }

    [System.Serializable]
    public struct Level
    {
        [ValueDropdown("GetScenesInBuildSettings")]
        public string LevelId;
        public LevelType LevelType;
        public LevelData LevelData;

    #if UNITY_EDITOR
        private IEnumerable<string> GetScenesInBuildSettings()
        {
            string[] scenes = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => Path.GetFileNameWithoutExtension(scene.path))
                .ToArray();

            return scenes;
        }
    #endif
    }
}
