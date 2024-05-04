using System.Collections;
using UnityEngine;

namespace VFX
{
    /// <summary>
    /// Changes opacity of the tint material repeatedly to create a flashing effect
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class DamageFlash : MonoBehaviour
    {
        /* ------------------- Static Fields ------------------- */
        
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int TintColor = Shader.PropertyToID("_TintColor");
        
        /* ------------------- Public Fields ------------------- */
        
        /// <summary>
        /// Intensity of the tint over time
        /// </summary>
        public AnimationCurve intensityCurve;
        
        /* ------------------- Private Fields ------------------- */

        private SpriteRenderer _spriteRenderer;         // Sprite renderer of the object
        private Material _material;                     // Material of the sprite renderer
        private Color _tintColor = Color.cyan;          // Color of the tint
        private float _timeOfHit;                       // Time when the flashing started
    
        /* ------------------- Unity Callbacks ------------------- */
        
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _material = new Material(_spriteRenderer.material);
            _spriteRenderer.material = _material;
        
            _material.SetFloat(Opacity, 0);
        }
    
        /* ------------------- Public Methods ------------------- */
        
        /// <summary>
        /// Starts the flashing effect
        /// </summary>
        /// <param name="projectile"> (Not yet implemented) </param>
        public void StartFlashing(GameObject projectile)
        {
            StopFlashing();
            
            _timeOfHit = Time.time;
            _material.SetColor(TintColor, _tintColor);
        
            StartCoroutine(Flash());
        }
        
        /// <summary>
        /// Stops the flashing effect
        /// </summary>
        public void StopFlashing()
        {
            StopCoroutine(Flash());
            _material.SetFloat(Opacity, 0);
        }

        /* ------------------- Private Methods ------------------- */
        
        // Coroutine that sets the opacity of the tint according to the intensity curve
        private IEnumerator Flash()
        {
            float timeElapsed = Time.time - _timeOfHit;
        
            while (timeElapsed < intensityCurve.keys[^1].time)
            {
                timeElapsed = Time.time - _timeOfHit;
                float intensity = intensityCurve.Evaluate(timeElapsed);
                _material.SetFloat(Opacity, intensity);
                yield return null;
            }
        }
        
    }
}
