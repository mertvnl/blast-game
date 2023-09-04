using Core.Managers;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Systems
{
    public static class Bootstrapper
    {
        private const string MANAGERS_OBJECT_NAME = "Managers";
        private const string UI_OBJECT_NAME = "UI";

        /// <summary>
        /// Instantiates Managers and UI before game start in order to make sure that is managers are ready before game loads.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static async void Boot()
        {
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load(MANAGERS_OBJECT_NAME)));
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load(UI_OBJECT_NAME)));

            await Task.Delay(1000);

    #if UNITY_EDITOR
            //Only for testing purposes.
            LevelManager.Instance.LoadCurrentEditorLevel();
#else
            LevelManager.Instance.LoadLastLevel();
#endif
        }
    }
}