using System.Collections;
using System;
using UnityEngine;

namespace SeroJob.BroadcastingSystem
{
    public class BroadcastingButtonAnimator : MonoBehaviour
    {
        public enum ButtonAnimType
        {
            None,
            Scale
        }

        public void StartAnimatingButtonScale(float animSpeed, float startScale, float targetScale, Action onAnimEnd)
        {
            StartCoroutine(AnimateButtonScale(animSpeed, startScale, targetScale, onAnimEnd));
        }

        private IEnumerator AnimateButtonScale(float animSpeed, float startScale, float targetScale, Action onAnimEnd)
        {
            float t = 0.0f;
            transform.localScale = Vector3.one * startScale;

            while (t <= 1.0f)
            {
                t += Time.deltaTime * animSpeed;

                transform.localScale = Vector3.one * Mathf.Lerp(startScale, targetScale, t);

                yield return null;
            }

            onAnimEnd?.Invoke();
        }
    }
}