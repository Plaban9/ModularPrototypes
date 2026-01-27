using System.Collections;

using UnityEngine;

namespace ModularPrototypes.Scene.Transition
{
    public abstract class SceneTransition : MonoBehaviour
    {
        public abstract IEnumerator AnimateTransitionIn(float duration = 1f);
        public abstract IEnumerator AnimateTransitionOut(float duration = 1f);
    }
}