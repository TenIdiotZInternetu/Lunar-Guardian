using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VFX
{
    /// <summary>
    /// Smoothly pans the camera to a target position
    /// </summary>
    public class PanCamera : MonoBehaviour
    {
        /*-------------------- Public Fields --------------------*/
        
        /// <summary>
        /// Camera to pan
        /// </summary>
        public Camera camera;
        
        /// <summary>
        /// Final position of the camera after the pan
        /// </summary>
        public Vector3 targetPosition;
        
        /// <summary>
        /// Portion of the distance travelled by the camera towards the target position over time
        /// </summary>
        public AnimationCurve curve;

        /*-------------------- Unity Callbacks --------------------*/
        
        /// <summary>
        /// Starts smoothly moving the camera to the target position
        /// </summary>
        public void PanToPosition()
        {
            StartCoroutine(Pan());
        }

        
        /*-------------------- Private Methods --------------------*/
        
        // Coroutine that smoothly moves the camera to the target position
        private IEnumerator Pan()
        {
            float timePassed = 0;
            Vector3 startPosition = camera.transform.position;
            
            while (timePassed < curve.keys[curve.length - 1].time)
            {
                timePassed += Time.deltaTime;
                float t = curve.Evaluate(timePassed);
                camera.transform.position = Vector3.LerpUnclamped(startPosition, targetPosition, t);
                yield return null;
            }
        }
    }
}