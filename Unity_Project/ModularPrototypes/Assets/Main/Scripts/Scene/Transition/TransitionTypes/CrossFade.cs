using DG.Tweening;

using System.Collections;

using UnityEngine;

namespace ModularPrototypes.Scene.Transition.TransitionTypes
{
    public class CrossFade : SceneTransition
    {
        [SerializeField] private CanvasGroup _crossFade;

        public override IEnumerator AnimateTransitionIn(float duration = 1f)
        {
            _crossFade.blocksRaycasts = true;
            var tweener = _crossFade.DOFade(1f, duration);

            yield return tweener.WaitForCompletion();
        }

        public override IEnumerator AnimateTransitionOut(float duration = 1f)
        {
            _crossFade.blocksRaycasts = false;
            var tweener = _crossFade.DOFade(0f, duration);

            yield return tweener.WaitForCompletion();
        }
    }
}
