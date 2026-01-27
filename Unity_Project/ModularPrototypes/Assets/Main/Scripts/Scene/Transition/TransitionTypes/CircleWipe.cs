using DG.Tweening;

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Scene.Transition.TransitionTypes
{
    public class CircleWipe : SceneTransition
    {
        [SerializeField] private Image _circle;

        public override IEnumerator AnimateTransitionIn(float duration = 1f)
        {
            _circle.rectTransform.anchoredPosition = new Vector2(-_circle.rectTransform.sizeDelta.x, duration);
            var tweener = _circle.rectTransform.DOAnchorPosX(0f, duration);

            yield return tweener.WaitForCompletion();
        }

        public override IEnumerator AnimateTransitionOut(float duration = 1f)
        {
            var tweener = _circle.rectTransform.DOAnchorPosX(_circle.rectTransform.sizeDelta.x, duration);

            yield return tweener.WaitForCompletion();
        }
    }
}
