using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Spawnables
{
    /// <summary>
    /// A set of pre-instantiated entities of a specified prefab
    /// that can be spawned and despawned by the ObjectPoolManager
    /// </summary>
    [Serializable]
    public class ObjectPool
    {
        /* -------------------- Static Fields -------------------- */
        
        private static Transform _parentTransform;  // A parent transform for all the objects in the pool
        
        /* --------------------- Properties ---------------------- */
        
        /// <summary>
        /// A key to the Dictionary of all the ObjectPools required to extract an entity from the pool   
        /// </summary>
        public string Key => prefab.GetComponent<Entity>().SpawnKey;
        
        /* -------------------- Public Fields -------------------- */
        
        /// <summary>
        /// Object prefab stored in the pool
        /// </summary>
        public GameObject prefab;

        /// <summary>
        /// Amount of objects to instantiate when the pool is created
        /// </summary>
        public int initialPoolSize;
        
        /// <summary>
        /// Maximum amount of objects that can be stored in the pool
        /// </summary>
        public int maxPoolSize;
    
        /* -------------------- Private Fields -------------------- */
    
        private Queue<GameObject> _pool = new();    // Queue of available objects in the pool
        private int _poolSize;                      // Current amount of objects instantiated in the pool
        private int _availableInstances;            // Amount of objects available for extraction from the queue
        
        
        /* -------------------- Static Methods -------------------- */
        
        /// <summary>
        /// Sets a parent transform under which new objects in the pool are instantiated
        /// </summary>
        /// <param name="parentTransform"></param>
        public static void SetParentTransform(Transform parentTransform)
        {
            _parentTransform = parentTransform;
        }
        
        /* -------------------- Public Methods -------------------- */
        
        /// <summary>
        /// Instantiates objects and adds them to the pool, to fill it up to the initialPoolSize
        /// </summary>
        public void Populate()
        {
            _poolSize = initialPoolSize;
        
            for (int i = 0 ; i < initialPoolSize ; i++)
            {
                GameObject obj = Object.Instantiate(prefab, _parentTransform, false);
                obj.name = Key;
                Enqueue(obj);
                obj.SetActive(false);
            }
        }
        
        /// <summary>
        /// Adds an object to the queue of available objects in the pool
        /// </summary>
        /// <param name="obj"> Added object </param>
        public void Enqueue(GameObject obj)
        {
            _pool.Enqueue(obj);
            _availableInstances = _pool.Count;
        }
    
        /// <summary>
        /// Retrieves an object from the queue. If the queue is empty, instantiates a new object.
        /// </summary>
        /// <returns> Object retrieved from the pool </returns>
        /// <exception cref="OperationCanceledException"> The maximum amount of objects in the pool has been exceeded </exception>
        public GameObject Extract()
        {
            GameObject obj = null;

            if (_availableInstances > 0)
            {
                obj = _pool.Dequeue();
            }
            else if (_poolSize <= maxPoolSize)
            {
                obj = Object.Instantiate(prefab, _parentTransform, false);
                _poolSize++;
            }
            else
            {
                throw new OperationCanceledException("Maximum pool size exceeded!");
            }
        
            _availableInstances = _pool.Count;
            return obj;
        }
    
        
        
    }
}