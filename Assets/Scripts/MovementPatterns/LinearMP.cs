using System;
using MyBox;
using Spawnables;
using UnityEngine;

namespace MovementPatterns
{
    /// <summary>
    /// Moves along a straight line
    /// </summary>
    [Serializable]
    public class LinearMP : MovementPattern
    {
        /* ------------------- Inspector Variables ------------------- */
        
        /// <summary>
        /// Distance travelled in one second
        /// </summary>
        [SerializeField] private float speed;
        
        /// <summary>
        /// Rate of change of speed
        /// </summary>
        [SerializeField] private float acceleration;
        
        /// <summary>
        /// If true, the slope of the line is determined by the entity's rotation
        /// </summary>
        [SerializeField] private bool followsRotation;
        
        /// <summary>
        /// If followsRotation is false, this is the angle of the slope in degrees, where 0 is up
        /// </summary>
        [SerializeField, ConditionalField(nameof(followsRotation), inverse: true)]
        private float directionInDegrees;
        
        
        /* ---------------------- Private Fields ---------------------- */
        
        private Vector3 _direction;

        /* ---------------------- Public Methods ---------------------- */
        
        public override void Initialize(Entity entity)
        {
            OnValidate();
        }

        public override Vector3 GetNextPosition(Entity entity)
        {
            Vector3 localDirection = _direction;
            
            // If followsRotation is true, the direction is determined by the entity's rotation
            if (followsRotation)
            {
                float rotation = entity.transform.rotation.eulerAngles.z;
                localDirection = Quaternion.Euler(0, 0, rotation) * Vector3.up;
                localDirection = localDirection.normalized;
            }
            
            Vector3 currentPosition = entity.transform.position;
            
            float currentSpeed = Mathf.Max(speed + acceleration * entity.MPLifeTime, 0);
            return currentPosition + localDirection * (currentSpeed * Time.deltaTime);
        }
        
        /* --------------------- Editor-only callbacks ---------------------- */
        
        // Changes angle of the slope, so it can be immediately seen in the editor
        void OnValidate()
        {
            _direction = Quaternion.Euler(0, 0, directionInDegrees) * Vector3.up;
            _direction = _direction.normalized;
        }
    
        // Draws a line in the editor to show the direction of the slope
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector3 position = transform.position;
            Gizmos.DrawLine(position, position + _direction);
        }
    }
}
