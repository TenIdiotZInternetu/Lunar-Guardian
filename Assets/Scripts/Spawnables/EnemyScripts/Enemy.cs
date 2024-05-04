using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawnables.EnemyScripts
{
    /// <summary>
    /// Defininf characteristics of an enemy entity
    /// </summary>
    [Serializable]
    [RequireComponent(typeof(EnemyController))]
    public class Enemy : Entity
    {
        /* ---------------------- Types ---------------------- */
        
        /// <summary>
        /// Described possible loot drops from the enemy
        /// </summary>
        [Serializable]
        public class LootDrop
        {
            /// <summary>
            /// Prefab of the item to be dropped
            /// </summary>
            public GameObject item;
            
            /// <summary>
            /// Chance of the item being dropped on death
            /// </summary>
            public float dropChance;
            
            /// <summary>
            /// Max amount of items of this type to be dropped
            /// </summary>
            public int maxDrops;
        }
        
        /* ------------------- Public Fields ------------------- */
        
        /// <summary>
        /// Whether the enemy rotates towards the player
        /// </summary>
        public bool tracksPlayer = false;
    
        /// <summary>
        /// Maximum health of the enemy. Health at spawn.
        /// </summary>
        public int maxHealth;
        
        /// <summary>
        /// Score reward for killing this enemy
        /// </summary>
        public int scoreReward;
        
        /// <summary>
        /// List of items that can be dropped on death
        /// </summary>
        public List<LootDrop> drops;
    }
}