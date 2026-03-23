using UnityEngine;

namespace ModularPrototypes.Scene
{
    public class SceneCallback : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;

        public void OnAnimationFinished()
        {
            if (_sceneLoader == null)
            {
                return;
            }

            _sceneLoader.LoadScene(SceneModules.Main, "CrossFade");
        }
    }
}
