using System;
using Spawnables;
using UnityEngine;

namespace MovementPatterns
{
    /// <summary>
    /// Defines calculations for the position of an entity in the next frame based on its properties
    /// </summary>
    [Serializable]
    public abstract class MovementPattern : MonoBehaviour
    {
        /* ------------------- Public Methods ------------------- */
        
        /// <summary>
        /// Sets parameters of the movement pattern based on the entity's state
        /// </summary>
        /// <param name="entity"> Entity to adjust the parameters to </param>
        public abstract void Initialize(Entity entity);
        
        /// <summary>
        /// Calculates the position of the entity in the next frame
        /// </summary>
        /// <param name="entity"> Entity of which position is calculated </param>
        /// <returns> The calculated position </returns>
        public abstract Vector3 GetNextPosition(Entity entity);
    }
}
