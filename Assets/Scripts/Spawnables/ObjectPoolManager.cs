using System.Collections.Generic;
using System.IO;
using Tools;
using UnityEngine;

namespace Spawnables
{
    /// <summary>
    /// Manages spawning and despawning entities from the assigned ObjectPools
    /// </summary>
    public class ObjectPoolManager : MonoBehaviour
    {
        /* -------------------- Static Fields -------------------- */
        
        /// <summary>
        /// Singleton instance of the ObjectPoolManager
        /// </summary>
        public static ObjectPoolManager Instance;

        // Table of Key-Pool pairs for quick access to the pools
        private static Dictionary<string, ObjectPool> _poolTable;      
        
        /* -------------------- Public Fields -------------------- */
        
        /// <summary>
        /// List of all available ObjectPools
        /// </summary>
        public List<ObjectPool> objectPools;

        /* -------------------- Unity Callbacks -------------------- */
        
        // Populates the pools and adds them to the pool table
        void Start()
        {
            Instance = this;
            _poolTable = new Dictionary<string, ObjectPool>();
            ObjectPool.SetParentTransform(transform);
        
        
            foreach (var pool in objectPools)
            {
                _poolTable.Add(pool.Key, pool);
                pool.Populate();
            }
        }
    
        /* -------------------- Static Methods -------------------- */
        
        /// <summary>
        /// Activates an entity extracted from a pool in the pool table. Replaces the Entity component of the
        /// object from the pool with the from the prefab
        /// </summary>
        /// <param name="prefab"> Entity to be spawned </param>
        /// <param name="position"> Position at which the entity should be spawned </param>
        /// <param name="rotation"> Rotation of the spawned entity </param>
        /// <typeparam name="T"> Type inheriting from Entity </typeparam>
        /// <returns> The Spawned Entity </returns>
        public static GameObject Spawn<T>(GameObject prefab, Vector3 position, Quaternion rotation) where T : Entity
        {
            T prefabEntity = prefab.GetComponent<T>();
        
            if (prefabEntity == null)
            {
                throw new InvalidDataException("The prefab does not contain a component of type Entity");
            }
        
            string spawnKey = prefabEntity.SpawnKey;
            GameObject spawnedObject = _poolTable[spawnKey].Extract();
        
            spawnedObject.transform.position = position;
            spawnedObject.transform.rotation = rotation;
        
            T objEntity = spawnedObject.GetComponent<T>();
            ComponentUtils.ReplaceComponent<T>(objEntity, prefabEntity);

            spawnedObject.SetActive(true);

            foreach (Transform child in spawnedObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        
            return spawnedObject;
        }

        /// <summary>
        /// Returns an entity back to the pool
        /// </summary>
        /// <param name="obj"> Entity to be returned </param>
        /// <returns> The returned entity </returns>
        public static GameObject Despawn(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.position = Instance.transform.position;
        
            string spawnKey = obj.GetComponent<Entity>().SpawnKey;
            _poolTable[spawnKey].Enqueue(obj);

            return obj;
        }
    }
}