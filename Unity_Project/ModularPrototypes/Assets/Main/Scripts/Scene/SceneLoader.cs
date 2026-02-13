using ModularPrototypes.Scene.Transition;

using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Scene
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject _transitionContainer;
        [SerializeField] private SceneTransition[] _transitions;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private string currentScene = SceneModules.Platformer_Prototype;

        private void Awake()
        {
            _transitions = _transitionContainer.GetComponentsInChildren<SceneTransition>();
        }

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

            StartCoroutine(LoadSceneAsync(sceneToLoad, _transitions[Random.Range(0, _transitions.Length)].name));
            currentScene = sceneToLoad;
        }

        public void LoadScene(string sceneToLoad, string transitionName = "CrossFade")
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

            StartCoroutine(LoadSceneAsync(sceneToLoad, transitionName));
            currentScene = sceneToLoad;
        }

        private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
        {
            SceneTransition transition = _transitions.First(element => element.name.Equals(transitionName));

            yield return transition.AnimateTransitionIn(0.5f);

            _progressBar.gameObject.SetActive(true);

            AsyncOperation scene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            scene.allowSceneActivation = false;

            do
            {
                _progressBar.value = Mathf.SmoothStep(_progressBar.value, scene.progress, .05f);
                yield return null;

            } while (scene.progress < .9f || _progressBar.value < 0.875f);

            scene.allowSceneActivation = true;
            _progressBar.gameObject.SetActive(false);

            yield return transition.AnimateTransitionOut(0.5f);
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