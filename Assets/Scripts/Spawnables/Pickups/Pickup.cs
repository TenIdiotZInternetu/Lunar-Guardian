using System;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

namespace Spawnables.Pickups
{
    /// <summary>
    /// Entity increasing player's resources when picked up
    /// </summary>
    public class Pickup : Entity
    {
        /* ---------------------- Types ---------------------- */
        
        /// <summary>
        /// Resources gained by touching the pickup
        /// </summary>
        [Serializable]
        public class Reward
        {
            /// <summary>
            /// Type of resource to be gained
            /// </summary>
            public PlayerStatus.ResourceType type;
            
            /// <summary>
            /// Amount of resource gained
            /// </summary>
            public int amount;
        }
        
        /// <summary>
        /// List of resources gained by touching the pickup
        /// </summary>
        [SerializeField] public List<Reward> rewards;
        
        /* ------------------- Unity Callbacks ------------------- */
        
        // Rewards the player and despawns when touched
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            GiveRewards();
            ObjectPoolManager.Despawn(gameObject);
        }
        
        /* ------------------- Private Methods ------------------- */

        // Gives the player the rewards
        private void GiveRewards()
        {
            foreach (Reward reward in rewards)
            {
                PlayerStatus.ChangeResource(reward.type, reward.amount);
            }
        }
    }
}