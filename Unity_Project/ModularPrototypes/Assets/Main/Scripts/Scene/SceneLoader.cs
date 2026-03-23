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

        /// <summary>
        /// Loads a new scene with a random transition. If there is a current scene, it will be unloaded before loading the new scene. <br/>
        /// 
        /// By default, 
        /// The new scene will be loaded in single mode and the progress bar will not be shown. If you want to load the new scene in additive mode or show the progress bar, use the overloaded LoadScene method.
        /// </summary>
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

            StartCoroutine(LoadSceneAsync(sceneToLoad, _transitions[Random.Range(0, _transitions.Length)].name, false, false));
            currentScene = sceneToLoad;
        }

        /// <summary>
        /// Loads a new scene with a random transition. If there is a current scene, it will be unloaded before loading the new scene. <br/>
        /// </summary>
        /// <param name="sceneToLoad">The name of the scene to load.</param>
        /// <param name="transitionName">The name of the transition to use. By default, it is "CrossFade".</param>
        /// <param name="isAdditive">Whether to load the new scene in additive mode or single mode. By default, it is false (single mode).</param>
        /// <param name="showProgress">Whether to show the progress bar while loading the new scene. By default, it is false (do not show progress bar).</param>
        public void LoadScene(string sceneToLoad, string transitionName = "CrossFade", bool isAdditive = false, bool showProgress = false, float animateTranstionInTimeInSecs = 0.5f, float animateTranstionOutTimeSecs = 0.5f)
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

            StartCoroutine(LoadSceneAsync(sceneToLoad, transitionName, isAdditive, showProgress, animateTranstionInTimeInSecs, animateTranstionOutTimeSecs));
            currentScene = sceneToLoad;
        }

        private IEnumerator LoadSceneAsync(string sceneName, string transitionName, bool isAdditive, bool showProgress, float animateTranstionInTimeInSecs = 0.5f, float animateTranstionOutTimeSecs = 0.5f)
        {
            SceneTransition transition = _transitions.First(element => element.name.Equals(transitionName));

            yield return transition.AnimateTransitionIn(animateTranstionInTimeInSecs);

            _progressBar.gameObject.SetActive(showProgress);

            AsyncOperation scene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, isAdditive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : UnityEngine.SceneManagement.LoadSceneMode.Single);
            scene.allowSceneActivation = false;

            do
            {
                _progressBar.value = Mathf.SmoothStep(_progressBar.value, scene.progress, .05f);
                yield return null;

            } while (scene.progress < .9f || _progressBar.value < 0.875f);

            scene.allowSceneActivation = true;
            _progressBar.gameObject.SetActive(false);

            yield return transition.AnimateTransitionOut(animateTranstionOutTimeSecs);
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