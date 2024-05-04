using UnityEngine;

namespace Spawnables.EnemyScripts
{
    /// <summary>
    /// Repeatedly spawns specified enemies at a given interval
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        /*-------------------- Constants --------------------*/
        
        private const float GIZMO_RADIUS = 0.5f;     // Radius of the gizmo sphere, marking position of spawner

        /*--------------=---- Public Fields ------------------*/
        
        /// <summary>
        /// A prefab of the enemy to spawn
        /// </summary>
        public GameObject prefab;
        
        /// <summary>
        /// Time between spawns in seconds
        /// </summary>
        public float interval = 1f;
        
        /// <summary>
        /// Total amount of enemies spawned during the spawner's lifetime
        /// </summary>
        public int maxCount;
        
        /*----------------- Private Variables -----------------*/
        
        private float _timeOfLastSpawn;               // Exact time in seconds
        private int _spawnedCount;                    // Amount of enemies spawned yet

        /*------------------ Unity Callbacks ------------------*/
        
        // Spawns next enemy if the spawner is not on cooldown and the max count is not reached
        private void Update()
        {
            bool onCooldown = Time.time - _timeOfLastSpawn < interval;
            if (onCooldown && _timeOfLastSpawn != 0) return;
            if (_spawnedCount >= maxCount) return;
            
            _timeOfLastSpawn = Time.time;
            _spawnedCount++;
            ObjectPoolManager.Spawn<Enemy>(prefab, transform.position, prefab.transform.rotation);
        }
        
        /*------------------ Private Methods ------------------*/
        
        // Draws a red sphere in the editor, marking position of the spawner
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GIZMO_RADIUS);
        }
    }
}