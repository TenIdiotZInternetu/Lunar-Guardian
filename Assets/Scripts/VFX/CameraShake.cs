using System.Collections;
using UnityEngine;

namespace VFX
{
    /// <summary>
    /// Shakes the camera violently, by displacing it rapidly over time
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        /*----------------- Public Fields -----------------*/
        
        /// <summary>
        /// Camera to shake
        /// </summary>
        public Camera camera;
        
        /// <summary>
        /// Intensity multiplier of the shake over time
        /// </summary>
        public AnimationCurve shakeCurve;
        
        /// <summary>
        /// Distance the camera will be displaced from its original position
        /// </summary>
        public float intensity;
        
        /// <summary>
        /// Amount of displacements per second
        /// </summary>
        public float refreshRate;
        
        /*----------------- Private Fields -----------------*/
        
        private Vector3 _originalPosition;          // Original position of the camera
        private float _displacementTime;            // Time between displacements
        
        private float _timeOfImpact;                // Time when the camera starts shaking
        private float _timeOfLastShake;             // Time when the camera was last displaced
        
        /*----------------- Unity Callbacks -----------------*/
        
        void Start()
        {
            _originalPosition = camera.transform.position;
            _displacementTime = 1 / refreshRate;
        }
        
        /*----------------- Public Methods -----------------*/
        /// <summary>
        /// Starts shaking the camera violently
        /// </summary>
        public void ShakeCamera()
        {
            StopCoroutine(DisplaceCamera());
            camera.transform.position = _originalPosition;
            _timeOfImpact = Time.time;
            StartCoroutine(DisplaceCamera());
        }
        
        /*----------------- Private Methods -----------------*/

        // Coroutine that displaces the camera over time
        private IEnumerator DisplaceCamera()
        {
            float timeElapsed = Time.time - _timeOfImpact;
        
            while (timeElapsed < shakeCurve.keys[^1].time)
            {
                timeElapsed = Time.time - _timeOfImpact;

                if (!(Time.time - _timeOfLastShake > _displacementTime))
                {
                    yield return null;
                    continue;
                }
                
                _timeOfLastShake = Time.time;
                float magnitude = intensity * shakeCurve.Evaluate(timeElapsed);
                Vector3 displacement = Random.insideUnitCircle.normalized * magnitude;
                
                camera.transform.position = _originalPosition + displacement;
                yield return null;
            }
            
            camera.transform.position = _originalPosition;
        }
    }
}