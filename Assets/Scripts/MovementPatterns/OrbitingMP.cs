using Spawnables;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace MovementPatterns
{
    /// <summary>
    /// Forms a circular orbit around targeted object
    /// </summary>
    public class OrbitingMP : MovementPattern
    {
        /* ------------------- Inspector Variables ------------------- */
        
        /// <summary>
        /// Targeted object to orbit around
        /// </summary>
        [SerializeField] private GameObject target;  
        
        /// <summary>
        /// Radius of the orbit
        /// </summary>
        [SerializeField] private float radius;
        
        /// <summary>
        /// Initial position of the entity on the orbit in degrees
        /// </summary>
        [SerializeField] private float phase;
        
        /// <summary>
        /// Angular speed in degrees per second
        /// </summary>
        [SerializeField] private float angularSpeed;
        
        /// <summary>
        /// Rate of change of angular speed
        /// </summary>
        [SerializeField] private float acceleration;
        
        /* ---------------------- Public Methods ---------------------- */
        public override void Initialize(Entity entity) { }

        public override Vector3 GetNextPosition(Entity entity)
        {
            // Ensures that the entity despawns when the target despawns
            if (!target.gameObject.activeInHierarchy)
            {
                ObjectPoolManager.Despawn(this.gameObject);
            }
            
            float time = entity.MPLifeTime;
            Vector3 center = target.transform.position;
            
            float currentSpeed = angularSpeed + acceleration * time;
            float angle = currentSpeed * time + phase;
            Vector3 slope = Quaternion.Euler(0, 0, angle) * Vector3.up * radius;

            return center + slope;
        }
        
        /* ------------------- Editor-only Callbacks ------------------ */
        
        #if UNITY_EDITOR
        // Draws the shape of the orbit in the editor
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.green;
            Vector3 center = target.transform.position;
            Handles.DrawWireDisc(center, Vector3.forward, radius);
        }
        #endif
    }
}