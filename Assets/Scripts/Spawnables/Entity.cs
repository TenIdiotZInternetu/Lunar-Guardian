using System;
using MovementPatterns;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Spawnables
{
    /// <summary>
    /// An object that can be spawned from the ObjectPool and moved by the MovementPattern class
    /// </summary>
    public class Entity : MonoBehaviour
    {
        /*-------------------- Properties --------------------*/
        
        /// <summary>
        /// Time passed since the entity was enabled
        /// </summary>
        public float LifeTime => Time.time - _timeEnabled;
        
        /// <summary>
        /// Time passed since the MovementPattern was last set
        /// </summary>
        public float MPLifeTime => Time.time - _timeMPChanged;
        
        /*-------------------- Public Fields --------------------*/
        
        /// <summary>
        /// Movement pattern that determines the position of the entity in the next frame
        /// </summary>
        public MovementPattern movementPattern;
        
        /// <summary>
        /// A key to identify the entity among the ObjectPools
        /// </summary>
        public string SpawnKey;

        /*-------------------- Private Fields --------------------*/
        
        private bool _isMoving = true;      // True if the entity should be moved by the MovementPattern
        private float _timeEnabled;         // Time when the entity was enabled in seconds
        private float _timeMPChanged;       // Time when the MovementPattern was last set in seconds

        /*-------------------- Unity Callbacks --------------------*/
        
        protected void OnEnable()
        {
            _timeEnabled = Time.time;

            if (movementPattern != null)
            {
                SetMovementPattern(movementPattern);
            }
        }
        
        protected void Update()
        {
            if (movementPattern != null && _isMoving)
            {
                transform.position = movementPattern.GetNextPosition(this);
            }
        }
        
        /* -------------------- Public Methods -------------------- */
        
        /// <summary>
        /// Changes the MovementPattern of the entity
        /// </summary>
        /// <param name="newMovementPattern"> The new movement pattern </param>
        public void SetMovementPattern(MovementPattern newMovementPattern)
        {
            movementPattern = newMovementPattern;
            movementPattern.Initialize(this);
            _timeMPChanged = Time.time;
        }
        
        /// <summary>
        /// Tells the entity to start using its MovementPattern
        /// </summary>
        public void StartMoving() => _isMoving = true;
        
        /// <summary>
        /// Tells the entity to stop using its MovementPattern
        /// </summary>
        public void StopMoving() => _isMoving = false;
    }
}