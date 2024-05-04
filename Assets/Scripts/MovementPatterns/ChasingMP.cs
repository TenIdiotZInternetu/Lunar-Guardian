using Spawnables;
using UnityEngine;

namespace MovementPatterns
{
    /// <summary>
    /// Entity chases a target for a set amount of time and then continues in a straight line.
    /// </summary>
    public class ChasingMP : MovementPattern
    {
        /* ------------------- Inspector Variables ------------------- */
        
        /// <summary>
        /// The target being chased
        /// </summary>
        [SerializeField] private GameObject target;
        
        /// <summary>
        /// Time before the entity starts chasing the target. Use to avoid unrealistic behavior upon spawning.
        /// </summary>
        [SerializeField] private float lockOnTime;
        
        /// <summary>
        /// Amount of time the entity chases the target before continuing in a straight line.
        /// </summary>
        [SerializeField] private float chaseTime;
        
        /// <summary>
        /// Rate at which the entity rotates towards the target.
        /// </summary>
        [SerializeField] private float attractionRate;
        
        /// <summary>
        /// Distance traveled after one second
        /// </summary>
        [SerializeField] private float speed;
        
        /// <summary>
        /// Rate of change of speed
        /// </summary>
        [SerializeField] private float acceleration;
        
        /* ---------------------- Public Methods ---------------------- */
        
        public override void Initialize(Entity entity) { }
        
        public override Vector3 GetNextPosition(Entity entity)
        {
            // Defining current state of the entity
            Transform entityTransform = entity.transform;
            
            Vector3 currentPosition = entityTransform.position;
            Quaternion currentRotation = entityTransform.rotation;
            float currentSpeed = speed + acceleration * entity.MPLifeTime;
            
            // If the entity is not locked on the target, it continues in a straight line
            if (entity.MPLifeTime < lockOnTime || entity.MPLifeTime > lockOnTime + chaseTime)
            {
                return currentPosition + currentRotation * Vector3.up * (currentSpeed * Time.deltaTime);
            }

            // Defining desired state
            Vector3 targetPosition = target.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetPosition - currentPosition);
            
            // Difference between current and desired state
            float rotationDifference = Quaternion.Angle(currentRotation, targetRotation);
            float distanceFromTarget = Mathf.Max(Vector3.Distance(currentPosition, targetPosition), 1.2f);

            // Calculating rotation of the entity towards the target
            float distanceFactor = 1 + 1 / Mathf.Log(distanceFromTarget, 5);
            float slerpRate = attractionRate * Time.deltaTime * rotationDifference * distanceFactor;
            Quaternion finalRotation = Quaternion.RotateTowards(currentRotation, targetRotation, slerpRate);
            
            
            // Calculating final position of the entity
            Vector3 momentum = finalRotation * Vector3.up * (currentSpeed * Time.deltaTime);
            entityTransform.rotation = finalRotation;
            
            Vector3 finalPosition = currentPosition + momentum;
            return finalPosition;
        }
    }
}