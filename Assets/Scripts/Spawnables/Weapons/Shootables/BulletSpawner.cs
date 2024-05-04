using UnityEngine;

namespace Spawnables.Weapons
{
    /// <summary>
    /// Spawns a specified projectile when shot either by the player or an enemy
    /// </summary>
    public class BulletSpawner : MonoBehaviour, IShootable
    {
        /* --------------------- Constant --------------------- */
        
        private const float GIZMO_RADIUS = 0.05f;
        
        /* ------------------ Public Fields ------------------- */
        
        /// <summary>
        /// Projectile to spawn
        /// </summary>
        public GameObject projectile;
        
        /* ------------------ Private Fields ------------------ */
        
        // Transform of the spawner
        private Transform _thisTransform;
        
        /* ------------------ Unity Callbacks ----------------- */
        
        public void Awake()
        {
            _thisTransform = GetComponent<Transform>();
        }

        /* ------------------ Public Methods ------------------ */
        
        /// <summary>
        /// Spawn a projectile at the spawner's position and rotation
        /// </summary>
        public void OnShoot()
        {
            ObjectPoolManager.Spawn<Projectile>(projectile, _thisTransform.position, _thisTransform.rotation);
        }

        /* ------------------ Private Methods ----------------- */
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, GIZMO_RADIUS);
        }
    }
}
