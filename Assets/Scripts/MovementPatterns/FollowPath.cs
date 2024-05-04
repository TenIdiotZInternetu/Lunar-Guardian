using PathCreation;
using PathCreation.Examples;
using Spawnables;
using UnityEngine;

namespace MovementPatterns
{
    /// <summary>
    /// Utilizes PathCreator tool created by Sebastian Lague. Traces a path and moves along it.
    /// </summary>
    public class FollowPath : MovementPattern
    {
        /* ------------------- Inspector Variables ------------------- */
        /// <summary>
        /// Path to move along
        /// </summary>
        [SerializeField] private PathCreator path;
        
        /// <summary>
        /// Distance travelled in one second
        /// </summary>
        [SerializeField] private float speed = 1;
        
        /// <summary>
        /// Rate of change of speed
        /// </summary>
        [SerializeField] private float acceleration = 0;
        
        
        /* ---------------------- Public Methods ---------------------- */
        
        public override void Initialize(Entity entity) { }

        public override Vector3 GetNextPosition(Entity entity)
        {
            float distanceTravelled = speed * entity.MPLifeTime;
            
            float currentSpeed = speed + acceleration * entity.MPLifeTime;
            return path.path.GetPointAtDistance(distanceTravelled);
        }
    }
}
