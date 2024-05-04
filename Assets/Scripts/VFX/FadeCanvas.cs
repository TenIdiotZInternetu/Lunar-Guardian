using System.Collections;
using UnityEngine;

namespace VFX
{
    /// <summary>
    /// Changes the alpha of a canvas to either 0 or 1 over time
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeCanvas : MonoBehaviour
    {
        /*-------------------- Public Fields --------------------*/
        
        /// <summary>
        /// Time before the FadeIn starts
        /// </summary>
        public float fadeInDelay;
        
        /// <summary>
        /// Time before the FadeOut starts
        /// </summary>
        public float fadeOutDelay;
        
        /*-------------------- Private Fields --------------------*/
        
        private CanvasGroup _canvas;            // Canvas to fade
        
        /*-------------------- Unity Callbacks --------------------*/
        
        void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
        }
        
        /*-------------------- Public Methods --------------------*/
        
        /// <summary>
        /// Changes the alpha of the canvas to 1 over time
        /// </summary>
        /// <param name="fadeInTime"> Time it takes to fully fade in </param>
        public void FadeIn(float fadeInTime)
        {
            StartCoroutine(LerpAlpha(1, fadeInTime, fadeInDelay));
        }
        
        /// <summary>
        /// Changes the alpha of the canvas to 0 over time
        /// </summary>
        /// <param name="fadeOutTime"> Time it takes to fully fade out </param>
        public void FadeOut(float fadeOutTime)
        {
            StartCoroutine(LerpAlpha(0, fadeOutTime, fadeOutDelay));

        }
        
        /*-------------------- Private Methods --------------------*/
        
        
        private IEnumerator LerpAlpha(float targetAlpha, float duration, float delay)
        {
            float currentAlpha = _canvas.alpha;
            float timePassed = 0;

            yield return new WaitForSeconds(delay);
            
            while (timePassed < duration)
            {
                _canvas.alpha = Mathf.Lerp(currentAlpha, targetAlpha, timePassed / duration);
                timePassed += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}