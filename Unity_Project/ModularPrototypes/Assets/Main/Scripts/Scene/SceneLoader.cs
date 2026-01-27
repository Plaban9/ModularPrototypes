using UnityEngine;

namespace ModularPrototypes.Scene
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string currentScene = SceneModules.Platformer_Prototype;

        public void LoadScene(string sceneToLoad)
        {
            if (string.IsNullOrEmpty(sceneToLoad))
            {
                return;
            }

            if (!string.IsNullOrEmpty(currentScene))
            {
                D("Unloading Scene: " + currentScene);
                UnloadScene(currentScene);
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            currentScene = sceneToLoad;
        }

        public void UnloadScene(string sceneToUnload)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        public string GetCurrentScene()
        {
            return currentScene;
        }

        public void SetCurrentScene(string sceneName)
        {
            currentScene = sceneName;
        }

        private static void D(string message)
        {
            DebugUtils.DebugInfo.Print("<<SceneLoader>> " + message);
        }
    }
}
