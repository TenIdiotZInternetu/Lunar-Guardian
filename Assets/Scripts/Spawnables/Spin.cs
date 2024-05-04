using System;
using UnityEngine;

namespace Spawnables
{
    /// <summary>
    /// Rotates the object each frame
    /// </summary>
    public class Spin : MonoBehaviour
    {
        /*-------------------- Public Fields --------------------*/
        
        /// <summary>
        /// Amount of degrees to rotate per second
        /// </summary>
        public float angularSpeed;

        
        /*-------------------- Unity Callbacks --------------------*/
        
        void Update()
        {
            transform.Rotate(0, 0, angularSpeed * Time.deltaTime);
        }
        
        /*-------------------- Public Methods --------------------*/
        
        /// <summary>
        /// Changes the direction of rotation from clockwise to counterclockwise or vice versa
        /// </summary>
        public void ChangeDirection()
        {
            angularSpeed *= -1;
        }
        
        /// <summary>
        /// Sets the angular speed to a new value
        /// </summary>
        /// <param name="newSpeed"> New value of the angular speed </param>
        public void ChangeSpeed(float newSpeed)
        {
            angularSpeed = newSpeed;
        }
    }
}